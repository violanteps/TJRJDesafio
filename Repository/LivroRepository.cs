using Dapper;
using Domain.Dtos;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Repository
{
    public class LivroRepository : ILivroRepository
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<LivroRepository> _logger;

        public LivroRepository(IDbConnection connection, ILogger<LivroRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }



        public async Task<int> CreateLivro(LivroEntity livroEntity, IDbTransaction transaction)
        {
            var query = @"INSERT INTO Livro (Titulo, Editora, Edicao, AnoPublicacao, StatusReg, DataCriacao, UltimaAtualizacao) 
                   VALUES (@Titulo, @Editora, @Edicao, @AnoPublicacao, @StatusReg, @DataCriacao, @UltimaAtualizacao);
                   SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                livroEntity.Titulo,
                livroEntity.Editora,
                livroEntity.Edicao,
                livroEntity.AnoPublicacao,
                StatusReg = 1,
                DataCriacao = DateTime.Now,
                UltimaAtualizacao = DateTime.Now
            };

            try
            {
                var livroId = await _connection.QuerySingleAsync<int>(query, parameters, transaction);
                return livroId;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao criar livro.");
                throw new Exception("Erro ao criar livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar livro.");
                throw;
            }
        }



        public async Task<LivroEntity> GetLivro(LivroEntity livroEntity)
        {
            var query = "SELECT Codl, Titulo, Editora, Edicao, AnoPublicacao, StatusReg FROM Livro WHERE Codl = @Codl and StatusReg = 1";

            try
            {
                var livro = await _connection.QueryFirstOrDefaultAsync<LivroEntity>(query, new { Codl = livroEntity.Codl });
                
                if(livro == null)
                    return new LivroEntity();
                
               
                return livro;
            
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar livro com Codl: {Codl}", livroEntity.Codl);
                throw new Exception("Erro ao acessar o banco de dados para obter o livro.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar livro com Codl: {Codl}", livroEntity.Codl);
                throw;
            }
        }


        public async Task<List<LivroEntity>> GetLivroList()
        {
            var query = "SELECT Codl, Titulo, Editora, Edicao, AnoPublicacao, StatusReg, DataCriacao FROM Livro WHERE StatusReg = 1";

            try
            {
                var livros = await _connection.QueryAsync<LivroEntity>(query);
                return livros.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar a lista de livros.");
                throw new Exception("Erro ao acessar o banco de dados para obter a lista de livros.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar a lista de livros.");
                throw;
            }
        }


        public async Task<bool> UpdateLivro(LivroEntity livroEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Livro 
                           SET Titulo = @Titulo, 
                               Editora = @Editora, 
                               Edicao = @Edicao, 
                               AnoPublicacao = @AnoPublicacao, 
                               StatusReg = @StatusReg, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE Codl = @Codl";

            var parameters = new
            {
                livroEntity.Titulo,
                livroEntity.Editora,
                livroEntity.Edicao,
                livroEntity.AnoPublicacao,
                StatusReg = 1,
                UltimaAtualizacao = DateTime.Now,
                livroEntity.Codl
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, $"Erro de banco de dados ao atualizar livro com Codl: {livroEntity.Codl}", sqlEx);
                throw new Exception("Erro ao atualizar livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar livro com Codl: {Codl}", livroEntity.Codl);
                throw;
            }
        }


        public async Task<bool> DeleteLivro(LivroEntity livroEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Livro 
                           SET StatusReg = 0, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE Codl = @Codl";

            var parameters = new
            {
                UltimaAtualizacao = DateTime.Now,
                livroEntity.Codl
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao deletar livro com Codl: {Codl}", livroEntity.Codl);
                throw new Exception("Erro ao deletar livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar livro com Codl: {Codl}", livroEntity.Codl);
                throw;
            }
        }




        public async Task<int> CreateAssunto(AssuntoEntity assuntoEntity, IDbTransaction transaction)
        {
            var query = @"INSERT INTO Assunto (Descricao, StatusReg, DataCriacao, UltimaAtualizacao) 
                           VALUES (@Descricao, @StatusReg, @DataCriacao, @UltimaAtualizacao);
                           SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                assuntoEntity.Descricao,
                StatusReg = 1,
                DataCriacao = DateTime.Now,
                UltimaAtualizacao = DateTime.Now
            };

            try
            {
                var assuntoId = await _connection.QuerySingleAsync<int>(query, parameters, transaction);
                return assuntoId;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao criar assunto.");
                throw new Exception("Erro ao criar assunto no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar assunto.");
                throw;
            }
        }


        public async Task<AssuntoEntity> GetAssunto(AssuntoEntity assuntoEntity)
        {
            var query = "SELECT * FROM Assunto WHERE CodAs = @CodAs AND StatusReg = 1";

            try
            {
                var assunto = await _connection.QueryFirstOrDefaultAsync<AssuntoEntity>(query, new { CodAs = assuntoEntity.CodAs });
                
                if(assunto == null)
                    return new AssuntoEntity();
                
                
                return assunto;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                throw new Exception($"Erro ao buscar assunto com CodAs: {assuntoEntity.CodAs} no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                throw;
            }
        }


        public async Task<List<AssuntoEntity>> GetAssuntoList()
        {
            var query = "SELECT * FROM Assunto WHERE StatusReg = 1";

            try
            {
                var assuntos = await _connection.QueryAsync<AssuntoEntity>(query);
                return assuntos.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar a lista de assuntos.");
                throw new Exception("Erro ao buscar a lista de assuntos no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar a lista de assuntos.");
                throw;
            }
        }


        public async Task<bool> UpdateAssunto(AssuntoEntity assuntoEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Assunto 
                           SET Descricao = @Descricao, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE CodAs = @CodAs";

            var parameters = new
            {
                assuntoEntity.Descricao,
                UltimaAtualizacao = DateTime.Now,
                assuntoEntity.CodAs
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar o assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                throw new Exception("Erro ao atualizar o assunto no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar o assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                throw;
            }
        }


        public async Task<bool> DeleteAssunto(AssuntoEntity assuntoEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Assunto 
                           SET StatusReg = 0, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE CodAs = @CodAs";

            var parameters = new
            {
                UltimaAtualizacao = DateTime.Now,
                assuntoEntity.CodAs
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao deletar o assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                throw new Exception("Erro ao deletar o assunto no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar o assunto com CodAs: {CodAs}", assuntoEntity.CodAs);
                throw;
            }
        }



        public async Task<int> CreateAutor(AutorEntity autorEntity, IDbTransaction transaction)
        {
            var query = @"INSERT INTO Autor (Nome, StatusReg, DataCriacao, UltimaAtualizacao) 
                           VALUES (@Nome, @StatusReg, @DataCriacao, @UltimaAtualizacao);
                           SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                autorEntity.Nome,
                StatusReg = 1,
                DataCriacao = DateTime.Now,
                UltimaAtualizacao = DateTime.Now
            };

            try
            {
                var autorId = await _connection.QuerySingleAsync<int>(query, parameters, transaction);
                return autorId;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao criar autor.");
                throw new Exception("Erro ao criar autor no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar autor.");
                throw;
            }
        }


        public async Task<AutorEntity> GetAutor(AutorEntity autorEntity)
        {
            var query = "SELECT * FROM Autor WHERE CodAu = @CodAu AND StatusReg = 1";

            try
            {
                var autor = await _connection.QueryFirstOrDefaultAsync<AutorEntity>(query, new { CodAu = autorEntity.CodAu });
                if(autor == null)
                    return new AutorEntity();

            
                return autor;
            
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar autor com CodAu: {CodAu}", autorEntity.CodAu);
                throw new Exception("Erro ao buscar autor no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar autor com CodAu: {CodAu}", autorEntity.CodAu);
                throw;
            }
        }


        public async Task<List<AutorEntity>> GetAutorList()
        {
            var query = "SELECT * FROM Autor WHERE StatusReg = 1";

            try
            {
                var autores = await _connection.QueryAsync<AutorEntity>(query);
                return autores.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar a lista de autores");
                throw new Exception("Erro ao buscar lista de autores no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar a lista de autores");
                throw;
            }
        }


        public async Task<bool> UpdateAutor(AutorEntity autorEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Autor 
                           SET Nome = @Nome, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE CodAu = @CodAu";

            var parameters = new
            {
                autorEntity.Nome,
                UltimaAtualizacao = DateTime.Now,
                autorEntity.CodAu
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar autor com CodAu: {CodAu}", autorEntity.CodAu);
                throw new Exception("Erro ao atualizar autor no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar autor com CodAu: {CodAu}", autorEntity.CodAu);
                throw;
            }
        }


        public async Task<bool> DeleteAutor(AutorEntity autorEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Autor 
                           SET StatusReg = 0, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE CodAu = @CodAu";

            var parameters = new
            {
                UltimaAtualizacao = DateTime.Now,
                autorEntity.CodAu
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao deletar autor com CodAu: {CodAu}", autorEntity.CodAu);
                throw new Exception("Erro ao deletar autor no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar autor com CodAu: {CodAu}", autorEntity.CodAu);
                throw;
            }
        }



        public async Task<int> CreateTipoVenda(TipoVendaEntity tipoVendaEntity, IDbTransaction transaction)
        {
            var query = @"INSERT INTO TipoVenda (Nome, StatusReg, DataCriacao, UltimaAtualizacao) 
                           VALUES (@Nome, @StatusReg, @DataCriacao, @UltimaAtualizacao);
                           SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                tipoVendaEntity.Nome,
                StatusReg = 1,
                DataCriacao = DateTime.Now,
                UltimaAtualizacao = DateTime.Now
            };

            try
            {
                var tipoVendaId = await _connection.QuerySingleAsync<int>(query, parameters, transaction);
                return tipoVendaId;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao criar tipo de venda");
                throw new Exception("Erro ao criar tipo de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar tipo de venda");
                throw;
            }
        }


        public async Task<TipoVendaEntity> GetTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            var query = "SELECT * FROM TipoVenda WHERE Codv = @Codv AND StatusReg = 1";

            try
            {
                var tipoVenda = await _connection.QueryFirstOrDefaultAsync<TipoVendaEntity>(query, new { Codv = tipoVendaEntity.Codv });

                if(tipoVenda == null)
                    return new TipoVendaEntity();

                
                return tipoVenda;

            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar tipo de venda com Codv: {Codv}", tipoVendaEntity.Codv);
                throw new Exception("Erro ao buscar tipo de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar tipo de venda com Codv: {Codv}", tipoVendaEntity.Codv);
                throw;
            }
        }


        public async Task<List<TipoVendaEntity>> GetTipoVendaList()
        {
            var query = "SELECT * FROM Tipo_Venda WHERE StatusReg = 1";

            try
            {
                var tipoVendas = await _connection.QueryAsync<TipoVendaEntity>(query);
                return tipoVendas.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar a lista de tipos de venda");
                throw new Exception("Erro ao buscar a lista de tipos de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar a lista de tipos de venda");
                throw;
            }
        }


        public async Task<bool> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Tipo_Venda 
                           SET Nome = @Nome, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE Codv = @Codv";

            var parameters = new
            {
                tipoVendaEntity.Nome,
                UltimaAtualizacao = DateTime.Now,
                tipoVendaEntity.Codv
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar tipo de venda com Codv: {Codv}", tipoVendaEntity.Codv);
                throw new Exception("Erro ao atualizar tipo de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar tipo de venda com Codv: {Codv}", tipoVendaEntity.Codv);
                throw;
            }
        }


        public async Task<bool> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Tipo_Venda 
                           SET StatusReg = 0, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE Codv = @Codv";

            var parameters = new
            {
                UltimaAtualizacao = DateTime.Now,
                tipoVendaEntity.Codv
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao deletar tipo de venda com Codv: {Codv}", tipoVendaEntity.Codv);
                throw new Exception("Erro ao deletar tipo de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar tipo de venda com Codv: {Codv}", tipoVendaEntity.Codv);
                throw;
            }
        }


        public async Task<bool> CreateLivroValor(LivroValorEntity livroValorEntity, IDbTransaction transaction)
        {
            var query = @"INSERT INTO Livro_Valor (Livro_Codl, Venda_Codv, Valor_Venda, StatusReg, DataCriacao, UltimaAtualizacao) 
                           VALUES (@LivroCodl, @VendaCodv, @ValorVenda, @StatusReg, @DataCriacao, @UltimaAtualizacao);";

            var parameters = new
            {
                livroValorEntity.LivroCodl,
                livroValorEntity.VendaCodv,
                livroValorEntity.ValorVenda,
                StatusReg = 1,
                DataCriacao = DateTime.Now,
                UltimaAtualizacao = DateTime.Now
            };

            try
            {
                var ret = await _connection.ExecuteAsync(query, parameters, transaction);
                return ret > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao criar livro valor");
                throw new Exception("Erro ao criar valor do livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar livro valor");
                throw;
            }
        }

        public async Task<List<LivroValorEntity>> GetLivroValor(int livroCodl)
        {
            var query = @"SELECT Livro_Codl as LivroCodl,
                                  Venda_Codv as VendaCodv,
                                  CAST(Valor_Venda AS DECIMAL(10, 2)) as ValorVenda,
                                  StatusReg as StatusReg
                           FROM Livro_Valor 
                           WHERE Livro_Codl = @LivroCodl
                             AND StatusReg = 1;";

            try
            {
                var livroValor = await _connection.QueryAsync<LivroValorEntity>(query, new { LivroCodl = livroCodl });
                return livroValor.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar valor do livro para livro com Codl: {LivroCodl}", livroCodl);
                throw new Exception("Erro ao buscar valor do livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar valor do livro para livro com Codl: {LivroCodl}", livroCodl);
                throw;
            }
        }


        public async Task<List<LivroValorEntity>> GetLivroValorList()
        {
            var query = @"SELECT Livro_Codl as LivroCodl,
                                  Venda_Codv as VendaCodv,
                                  CAST(Valor_Venda AS DECIMAL(10, 2)) as ValorVenda,
                                  StatusReg as StatusReg
                           FROM Livro_Valor 
                           WHERE StatusReg = 1;";

            try
            {
                var livroValor = await _connection.QueryAsync<LivroValorEntity>(query);
                return livroValor.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar lista de valores de livros");
                throw new Exception("Erro ao buscar lista de valores de livros no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar lista de valores de livros");
                throw;
            }
        }


        public async Task<bool> UpdateLivroValor(LivroValorEntity livroValorEntity, IDbTransaction transaction)
        {
            var query = @"UPDATE Livro_Valor 
                           SET Valor_Venda = @ValorVenda, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE Livro_Codl = @LivroCodl 
                             AND Venda_Codv = @VendaCodv";

            var parameters = new
            {
                livroValorEntity.ValorVenda,
                UltimaAtualizacao = DateTime.Now,
                livroValorEntity.LivroCodl,
                livroValorEntity.VendaCodv
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao atualizar valor do livro para LivroCodl: {LivroCodl} e VendaCodv: {VendaCodv}", livroValorEntity.LivroCodl, livroValorEntity.VendaCodv);
                throw new Exception("Erro ao atualizar valor do livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar valor do livro para LivroCodl: {LivroCodl} e VendaCodv: {VendaCodv}", livroValorEntity.LivroCodl, livroValorEntity.VendaCodv);
                throw;
            }
        }


        public async Task<bool> DeleteLivroValor(int livroCodl, int vendaCodv, IDbTransaction transaction)
        {
            var query = @"UPDATE Livro_Valor 
                           SET StatusReg = 0, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE Livro_Codl = @LivroCodl 
                             AND Venda_Codv = @VendaCodv";

            var parameters = new
            {
                UltimaAtualizacao = DateTime.Now,
                LivroCodl = livroCodl,
                VendaCodv = vendaCodv
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao deletar livro valor para LivroCodl: {LivroCodl} e VendaCodv: {VendaCodv}", livroCodl, vendaCodv);
                throw new Exception("Erro ao deletar valor do livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar livro valor para LivroCodl: {LivroCodl} e VendaCodv: {VendaCodv}", livroCodl, vendaCodv);
                throw;
            }
        }


        public async Task<int> CreateLivroAutor(LivroAutorEntity livroAutorEntity, IDbTransaction transaction)
        {
            var query = @"INSERT INTO Livro_Autor (Livro_Codl, Autor_CodAu, StatusReg, DataCriacao, UltimaAtualizacao) 
                           VALUES (@LivroCodl, @AutorCodAu, @StatusReg, @DataCriacao, @UltimaAtualizacao);";

            var parameters = new
            {
                livroAutorEntity.LivroCodl,
                livroAutorEntity.AutorCodAu,
                StatusReg = 1,
                DataCriacao = DateTime.Now,
                UltimaAtualizacao = DateTime.Now
            };

            try
            {
                var livroAutorId = await _connection.QueryFirstOrDefaultAsync<int>(query, parameters, transaction);
                return livroAutorId;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao criar relação Livro-Autor");
                throw new Exception("Erro ao criar relação entre livro e autor no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar relação Livro-Autor");
                throw;
            }
        }


        public async Task<IEnumerable<LivroAutorEntity>> GetLivroAutor(int livroCodl)
        {
            var query = @"SELECT Livro_Codl as LivroCodl, Autor_CodAu as AutorCodAu, StatusReg
                           FROM Livro_Autor
                           WHERE Livro_Codl = @LivroCodl
                             AND StatusReg = 1";

            try
            {
                var livroAutores = await _connection.QueryAsync<LivroAutorEntity>(query, new { LivroCodl = livroCodl });
                return livroAutores;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar autores para livro com Codl: {LivroCodl}", livroCodl);
                throw new Exception("Erro ao buscar autores do livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar autores para livro com Codl: {LivroCodl}", livroCodl);
                throw;
            }
        }


        public async Task<IEnumerable<LivroAutorEntity>> GetLivroAutor(int livroCodl, IDbTransaction transaction = null)
        {
            var query = @"SELECT Livro_Codl as LivroCodl, Autor_CodAu as AutorCodAu, StatusReg
                   FROM Livro_Autor
                   WHERE Livro_Codl = @LivroCodl
                     AND StatusReg = 1";

            try
            {
                var livroAutores = await _connection.QueryAsync<LivroAutorEntity>(query, new { LivroCodl = livroCodl }, transaction: transaction);
                return livroAutores;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao buscar autores para livro com Codl: {LivroCodl}", livroCodl);
                throw new Exception("Erro ao buscar autores do livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar autores para livro com Codl: {LivroCodl}", livroCodl);
                throw;
            }
        }


        public async Task<bool> DeleteLivroAutor(int livroCodl, int autorCodAu, IDbTransaction transaction)
        {
            var query = @"UPDATE Livro_Autor 
                           SET StatusReg = 0, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE Livro_Codl = @LivroCodl 
                             AND Autor_CodAu = @AutorCodAu";

            var parameters = new
            {
                UltimaAtualizacao = DateTime.Now,
                LivroCodl = livroCodl,
                AutorCodAu = autorCodAu
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao deletar autor para livro com Codl: {LivroCodl} e AutorCodAu: {AutorCodAu}", livroCodl, autorCodAu);
                throw new Exception("Erro ao deletar autor do livro no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar autor para livro com Codl: {LivroCodl} e AutorCodAu: {AutorCodAu}", livroCodl, autorCodAu);
                throw;
            }
        }


        public async Task<int> CreateLivroAssunto(LivroAssuntoEntity livroAssuntoEntity, IDbTransaction transaction)
        {
            var query = @"INSERT INTO Livro_Assunto (Livro_Codl, Assunto_CodAs, StatusReg, DataCriacao, UltimaAtualizacao) 
                           VALUES (@LivroCodl, @AssuntoCodAs, @StatusReg, @DataCriacao, @UltimaAtualizacao);";

            var parameters = new
            {
                livroAssuntoEntity.LivroCodl,
                livroAssuntoEntity.AssuntoCodAs,
                StatusReg = 1,
                DataCriacao = DateTime.Now,
                UltimaAtualizacao = DateTime.Now
            };

            try
            {
                var livroAssuntoId = await _connection.QueryFirstOrDefaultAsync<int>(query, parameters, transaction);
                return livroAssuntoId;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao criar relação Livro-Assunto");
                throw new Exception("Erro ao criar relação entre livro e assunto no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar relação Livro-Assunto");
                throw;
            }
        }
    

    public async Task<LivroAssuntoEntity> GetLivroAssunto(int livroCodl)
    {
        var query = @"SELECT Livro_Codl as LivroCodl, Assunto_CodAs as AssuntoCodAs, StatusReg
                   FROM Livro_Assunto
                   WHERE Livro_Codl = @LivroCodl 
                     AND StatusReg = 1";

        try
        {
            var livroAssunto = await _connection.QueryFirstOrDefaultAsync<LivroAssuntoEntity>(query, new { LivroCodl = livroCodl });

                if( livroAssunto == null )
                    return new LivroAssuntoEntity();


            return livroAssunto;
        
            }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Erro de banco de dados ao buscar assuntos para livro com Codl: {LivroCodl}", livroCodl);
            throw new Exception("Erro ao buscar assuntos do livro no banco de dados.", sqlEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar assuntos para livro com Codl: {LivroCodl}", livroCodl);
            throw;
        }
    }


    public async Task<LivroAssuntoEntity> GetLivroAssunto(int livroCodl, IDbTransaction transaction = null)
    {
        var query = @"SELECT Livro_Codl as LivroCodl, Assunto_CodAs as AssuntoCodAs, StatusReg
                   FROM Livro_Assunto
                   WHERE Livro_Codl = @LivroCodl 
                     AND StatusReg = 1";

        try
        {
            var livroAssunto = await _connection.QueryFirstOrDefaultAsync<LivroAssuntoEntity>(query, new { LivroCodl = livroCodl }, transaction: transaction);

                if(livroAssunto == null)
                    return new LivroAssuntoEntity();


            return livroAssunto;
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Erro de banco de dados ao buscar assuntos para livro com Codl: {LivroCodl}", livroCodl);
            throw new Exception("Erro ao buscar assuntos do livro no banco de dados.", sqlEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar assuntos para livro com Codl: {LivroCodl}", livroCodl);
            throw;
        }
    }

        public async Task<bool> DeleteLivroAssunto(int livroCodl, int assuntoCodAs, IDbTransaction transaction)
        {
            var query = @"UPDATE Livro_Assunto 
                           SET StatusReg = 0, 
                               UltimaAtualizacao = @UltimaAtualizacao 
                           WHERE Livro_Codl = @LivroCodl 
                             AND Assunto_CodAs = @AssuntoCodAs";

            var parameters = new
            {
                UltimaAtualizacao = DateTime.Now,
                LivroCodl = livroCodl,
                AssuntoCodAs = assuntoCodAs
            };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters, transaction);
                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao deletar assunto para livro com Codl: {LivroCodl} e AssuntoCodAs: {AssuntoCodAs}", livroCodl, assuntoCodAs);
                throw new Exception("Erro ao deletar relação entre livro e assunto no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar assunto para livro com Codl: {LivroCodl} e AssuntoCodAs: {AssuntoCodAs}", livroCodl, assuntoCodAs);
                throw;
            }
        }


        public async Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio)
        {
            var query = "SELECT 1 as tipoRel, Autor, Livro, Assunto FROM vw_LivrosPorAutorComAssunto;";

            try
            {
                var result = await _connection.QueryAsync<RelLivrosPorAutorComAssuntoDTO>(query);
                return result.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao gerar relatório por autor com assunto.");
                throw new Exception("Erro ao gerar relatório por autor com assunto no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao gerar relatório por autor com assunto.");
                throw;
            }
        }


        public async Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio)
        {
            var query = "SELECT 2 as tipoRel, Autor, Livro, Assunto, Valor, TipoVenda FROM vw_LivrosPorAutorComValorETipoVenda";

            try
            {
                var result = await _connection.QueryAsync<RelLivrosPorAutorComValorETipoVendaDTO>(query);
                return result.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro de banco de dados ao gerar relatório por autor com valor e tipo de venda.");
                throw new Exception("Erro ao gerar relatório por autor com valor e tipo de venda no banco de dados.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao gerar relatório por autor com valor e tipo de venda.");
                throw;
            }
        }

    }
}
