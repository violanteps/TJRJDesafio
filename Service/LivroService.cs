using Domain.Dtos;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Repository;
using System.Data;
using System.Data.SqlClient;

namespace Service
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<LivroService> _logger;


        public LivroService(ILivroRepository livroRepository, IDbConnection dbConnection, ILogger<LivroService> logger)
        {
            _livroRepository = livroRepository;
            _dbConnection = dbConnection;
            _logger = logger;
        }

        private SqlConnection CreateConnection()
        {
            return (SqlConnection)_dbConnection;
        }

        public async Task<bool> CreateLivro(LivroEntity livroEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        if (livroEntity.LivroAutores == null || livroEntity.LivroAutores.Count == 0)
                            return false;

                        if (livroEntity.LivroAssuntoEntity == null || livroEntity.LivroAssuntoEntity.AssuntoCodAs == 0)
                            return false;


                        var livroId = await _livroRepository.CreateLivro(livroEntity, transaction);


                        var livroAssuntoEntity = new LivroAssuntoEntity
                        {
                            LivroCodl = livroId,
                            AssuntoCodAs = livroEntity.LivroAssuntoEntity.AssuntoCodAs,
                            StatusReg = 1
                        };
                        await _livroRepository.CreateLivroAssunto(livroAssuntoEntity, transaction);


                        foreach (var autor in livroEntity.LivroAutores)
                        {
                            var livroAutorEntity = new LivroAutorEntity
                            {
                                LivroCodl = livroId,
                                AutorCodAu = autor.AutorCodAu,
                                StatusReg = 1
                            };
                            await _livroRepository.CreateLivroAutor(livroAutorEntity, transaction);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao criar livro.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao criar livro.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }




        public async Task<LivroEntity> GetLivro(LivroEntity livroEntity)
        {
            try
            {
                var retGetLivro = await _livroRepository.GetLivro(livroEntity);

                if (retGetLivro == null)
                    return new LivroEntity();


                var retLivroAssunto = await _livroRepository.GetLivroAssunto(retGetLivro.Codl);
                var retLivroAutor = await _livroRepository.GetLivroAutor(retGetLivro.Codl);

                retGetLivro.LivroAssuntoEntity = retLivroAssunto;
                retGetLivro.LivroAutores = retLivroAutor.ToList();

                return retGetLivro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter livro com Codl: {Codl}", livroEntity.Codl);
                throw new InvalidOperationException($"Erro ao obter livro com Codl: {livroEntity.Codl}", ex);
            }
        }


        public async Task<List<LivroEntity>> GetLivroList()
        {
            try
            {
                var retGetLivroList = await _livroRepository.GetLivroList();

                if (retGetLivroList == null || retGetLivroList.Count == 0)
                    return new List<LivroEntity>();

                foreach (var livro in retGetLivroList)
                {
                    livro.LivroAssuntoEntity = await _livroRepository.GetLivroAssunto(livro.Codl);
                    livro.LivroAutores = (await _livroRepository.GetLivroAutor(livro.Codl)).ToList();
                }

                return retGetLivroList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lista de livros.");
                throw new InvalidOperationException("Erro ao obter lista de livros.", ex);
            }
        }


        public async Task<bool> UpdateLivro(LivroEntity livroEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (livroEntity.LivroAutores == null || livroEntity.LivroAutores.Count == 0)
                            return false;

                        if (livroEntity.LivroAssuntoEntity == null || livroEntity.LivroAssuntoEntity.AssuntoCodAs == 0)
                            return false;

                        await _livroRepository.UpdateLivro(livroEntity, transaction);

                        var livroAssuntoAtual = await _livroRepository.GetLivroAssunto(livroEntity.Codl, transaction);

                        if (livroAssuntoAtual == null || livroAssuntoAtual.AssuntoCodAs != livroEntity.LivroAssuntoEntity.AssuntoCodAs)
                        {
                            if (livroAssuntoAtual != null)
                            {
                                await _livroRepository.DeleteLivroAssunto(livroAssuntoAtual.LivroCodl, livroAssuntoAtual.AssuntoCodAs, transaction);
                            }

                            var livroAssuntoEntity = new LivroAssuntoEntity
                            {
                                LivroCodl = livroEntity.Codl,
                                AssuntoCodAs = livroEntity.LivroAssuntoEntity.AssuntoCodAs,
                                StatusReg = 1
                            };

                            await _livroRepository.CreateLivroAssunto(livroAssuntoEntity, transaction);
                        }

                        var autoresAtuais = await _livroRepository.GetLivroAutor(livroEntity.Codl, transaction);

                        foreach (var autorAtual in autoresAtuais)
                        {
                            if (!livroEntity.LivroAutores.Any(a => a.AutorCodAu == autorAtual.AutorCodAu))
                            {
                                await _livroRepository.DeleteLivroAutor(autorAtual.LivroCodl, autorAtual.AutorCodAu, transaction);
                            }
                        }

                        foreach (var autor in livroEntity.LivroAutores)
                        {
                            if (!autoresAtuais.Any(a => a.AutorCodAu == autor.AutorCodAu))
                            {
                                var livroAutorEntity = new LivroAutorEntity
                                {
                                    LivroCodl = livroEntity.Codl,
                                    AutorCodAu = autor.AutorCodAu,
                                    StatusReg = 1
                                };

                                await _livroRepository.CreateLivroAutor(livroAutorEntity, transaction);
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar livro com Codl: {Codl}", livroEntity.Codl);
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao atualizar livro com Codl: {Codl}", livroEntity.Codl);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<bool> DeleteLivro(LivroEntity livroEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var livroAssunto = await _livroRepository.GetLivroAssunto(livroEntity.Codl, transaction);
                        var livroAutores = await _livroRepository.GetLivroAutor(livroEntity.Codl, transaction);

                        if (livroAssunto != null)
                            await _livroRepository.DeleteLivroAssunto(livroAssunto.LivroCodl, livroAssunto.AssuntoCodAs, transaction);

                        foreach (var livroAutor in livroAutores)
                        {
                            await _livroRepository.DeleteLivroAutor(livroAutor.LivroCodl, livroAutor.AutorCodAu, transaction);
                        }

                        await _livroRepository.DeleteLivro(livroEntity, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao deletar livro com Codl: {Codl}", livroEntity.Codl);
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao deletar livro com Codl: {Codl}", livroEntity.Codl);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<bool> CreateAssunto(AssuntoEntity assuntoEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.CreateAssunto(assuntoEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao criar assunto.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao criar assunto.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<AssuntoEntity> GetAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {
                return await _livroRepository.GetAssunto(assuntoEntity);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao obter assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                throw new InvalidOperationException($"Erro ao acessar o banco de dados para obter o assunto com CodAs: {assuntoEntity.CodAs}", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                throw new InvalidOperationException($"Erro inesperado ao obter assunto com CodAs: {assuntoEntity.CodAs}", ex);
            }
        }


        public async Task<List<AssuntoEntity>> GetAssuntoList()
        {
            try
            {
                return await _livroRepository.GetAssuntoList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao obter a lista de assuntos.");
                throw new InvalidOperationException("Erro ao acessar o banco de dados para obter a lista de assuntos.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter a lista de assuntos.");
                throw new InvalidOperationException("Erro inesperado ao obter a lista de assuntos.", ex);
            }
        }


        public async Task<bool> UpdateAssunto(AssuntoEntity assuntoEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.UpdateAssunto(assuntoEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar o assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao atualizar o assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<bool> DeleteAssunto(AssuntoEntity assuntoEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.DeleteAssunto(assuntoEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao deletar o assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao deletar o assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<bool> CreateAutor(AutorEntity autorEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.CreateAutor(autorEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao criar autor.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao criar autor.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<AutorEntity> GetAutor(AutorEntity autorEntity)
        {
            try
            {
                return await _livroRepository.GetAutor(autorEntity);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao obter autor com CodAu: {CodAu}", autorEntity.CodAu);
                throw new InvalidOperationException($"Erro ao obter autor no banco de dados. CodAu: {autorEntity.CodAu}", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter autor com CodAu: {CodAu}", autorEntity.CodAu);
                throw new InvalidOperationException($"Erro inesperado ao obter autor com CodAu: {autorEntity.CodAu}", ex);
            }
        }


        public async Task<List<AutorEntity>> GetAutorList()
        {
            try
            {
                return await _livroRepository.GetAutorList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao obter lista de autores.");
                throw new InvalidOperationException("Erro ao obter lista de autores no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter lista de autores.");
                throw new InvalidOperationException("Erro inesperado ao obter lista de autores.", ex);
            }
        }


        public async Task<bool> UpdateAutor(AutorEntity autorEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.UpdateAutor(autorEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar autor.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao atualizar autor.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<bool> DeleteAutor(AutorEntity autorEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.DeleteAutor(autorEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao deletar autor.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao deletar autor.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<bool> CreateTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.CreateTipoVenda(tipoVendaEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao criar tipo de venda.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao criar tipo de venda.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<TipoVendaEntity> GetTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {
                return await _livroRepository.GetTipoVenda(tipoVendaEntity);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao obter tipo de venda com Codv: {Codv}", tipoVendaEntity.Codv);
                throw new InvalidOperationException($"Erro ao obter tipo de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter tipo de venda.");
                throw new InvalidOperationException($"Erro inesperado ao obter tipo de venda.", ex);
            }
        }

        public async Task<List<TipoVendaEntity>> GetTipoVendaList()
        {
            try
            {
                return await _livroRepository.GetTipoVendaList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao obter lista de tipos de venda.");
                throw new InvalidOperationException($"Erro ao obter lista de tipos de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter lista de tipos de venda.");
                throw new InvalidOperationException($"Erro inesperado ao obter lista de tipos de venda.", ex);
            }
        }


        public async Task<bool> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.UpdateTipoVenda(tipoVendaEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar tipo de venda.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao atualizar tipo de venda.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<bool> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.DeleteTipoVenda(tipoVendaEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao deletar tipo de venda.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao deletar tipo de venda.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<bool> CreateLivroValor(LivroValorEntity livroValorEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.CreateLivroValor(livroValorEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao criar valor do livro.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao criar valor do livro.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<List<LivroValorEntity>> GetLivroValor(int livroCodl)
        {
            try
            {
                return await _livroRepository.GetLivroValor(livroCodl);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao obter valor do livro com Codl: {Codl}", livroCodl);
                throw new InvalidOperationException($"Erro ao obter valor do livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter valor do livro.");
                throw new InvalidOperationException($"Erro inesperado ao obter valor do livro.", ex);
            }
        }


        public async Task<List<LivroValorEntity>> GetLivroValorList()
        {
            try
            {
                return await _livroRepository.GetLivroValorList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao obter lista de valores dos livros.");
                throw new InvalidOperationException($"Erro ao obter lista de valores dos livros no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao obter lista de valores dos livros.");
                throw new InvalidOperationException($"Erro inesperado ao obter lista de valores dos livros.", ex);
            }
        }

        public async Task<bool> UpdateLivroValor(LivroValorEntity livroValorEntity)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.UpdateLivroValor(livroValorEntity, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar valor do livro.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao atualizar valor do livro.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public async Task<bool> DeleteLivroValor(int livroCodl, int vendaCodv)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _livroRepository.DeleteLivroValor(livroCodl, vendaCodv, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        _logger.LogError(sqlEx, "Erro de banco de dados ao deletar valor do livro.");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao deletar valor do livro.");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public async Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio)
        {
            try
            {
                return await _livroRepository.GerarRelatorioPorAutorComAssunto(tipoRelatorio);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao gerar relatório por autor com assunto.");
                throw new InvalidOperationException("Erro ao gerar relatório por autor com assunto no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao gerar relatório por autor com assunto.");
                throw new InvalidOperationException("Erro inesperado ao gerar relatório por autor com assunto.", ex);
            }
        }


        public async Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio)
        {
            try
            {
                return await _livroRepository.GerarRelatorioPorAutorComValor(tipoRelatorio);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao gerar relatório por autor com valor e tipo de venda.");
                throw new InvalidOperationException("Erro ao gerar relatório por autor com valor e tipo de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao gerar relatório por autor com valor e tipo de venda.");
                throw new InvalidOperationException("Erro inesperado ao gerar relatório por autor com valor e tipo de venda.", ex);
            }
        }

    }
}
