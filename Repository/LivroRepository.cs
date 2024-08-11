using Dapper;
using Domain.Dtos;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    public class LivroRepository : ILivroRepository
    {

        private readonly string _connectionString;
        private readonly IDbConnection _connection;
        private readonly ILogger<LivroRepository> _logger;

        public LivroRepository(IDbConnection connection)
        {
            _connection = connection;
            _connectionString = _connection.ConnectionString;
        }



        public async Task<int> CreateLivro(LivroEntity livroEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  INSERT INTO Livro (Titulo, Editora, Edicao, AnoPublicacao, StatusReg, DataCriacao, UltimaAtualizacao) 
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

                            var livroId = await connection.QuerySingleAsync<int>(query, parameters, transaction: transaction);

                            transaction.Commit();

                            return livroId;
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar iniciar a transação: " + ex.Message);
            }
        }

        public async Task<LivroEntity> GetLivro(LivroEntity livroEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "SELECT Codl, Titulo, Editora, Edicao, AnoPublicacao, StatusReg FROM Livro WHERE Codl = @Codl and StatusReg = 1";
                    var livro = await connection.QueryFirstOrDefaultAsync<LivroEntity>(query, new { Codl = livroEntity.Codl });
                    return livro;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar o livro com Codl: {Codl}", livroEntity.Codl);
                throw new Exception("Erro ao acessar o banco de dados.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro desconhecido ao buscar o livro com Codl: {Codl}", livroEntity.Codl);
                throw new Exception("Erro ao processar a requisição.", ex);
            }
        }

        public async Task<List<LivroEntity>> GetLivroList()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "SELECT Codl, Titulo, Editora, Edicao, AnoPublicacao, StatusReg, DataCriacao FROM Livro WHERE StatusReg = 1";
                    var livro = await connection.QueryAsync<LivroEntity>(query);
                    return livro.ToList();
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar a lista de livros");
                throw new Exception("Erro ao acessar o banco de dados.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro desconhecido ao buscar a lista de livros");
                throw new Exception("Erro ao processar a requisição.", ex);
            }
        }

        public async Task<string> UpdateLivro(LivroEntity livroEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  UPDATE Livro 
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

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Livro atualizado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao atualizar livro";
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<string> DeleteLivro(LivroEntity livroEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  UPDATE Livro 
                                            SET StatusReg = 0, 
                                                UltimaAtualizacao = @UltimaAtualizacao 
                                            WHERE Codl = @Codl";

                            var parameters = new
                            {
                                UltimaAtualizacao = DateTime.Now,
                                livroEntity.Codl
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Livro deletado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao deletar livro";
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<int> CreateAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"
                        INSERT INTO Assunto (Descricao, StatusReg, DataCriacao, UltimaAtualizacao) 
                        VALUES (@Descricao, @StatusReg, @DataCriacao, @UltimaAtualizacao);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

                            var parameters = new
                            {
                                assuntoEntity.Descricao,
                                StatusReg = 1,
                                DataCriacao = DateTime.Now,
                                UltimaAtualizacao = DateTime.Now
                            };

                            var assuntoId = await connection.QuerySingleAsync<int>(query, parameters, transaction: transaction);

                            transaction.Commit();

                            return assuntoId;
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar iniciar a transação: " + ex.Message);
            }
        }

        public async Task<AssuntoEntity> GetAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT * FROM Assunto WHERE CodAs = @CodAs AND StatusReg = 1";
                    var assunto = await connection.QueryFirstOrDefaultAsync<AssuntoEntity>(query, new { CodAs = assuntoEntity.CodAs });
                    return assunto;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro de banco de dados: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o assunto: " + ex.Message, ex);
            }
        }

        public async Task<List<AssuntoEntity>> GetAssuntoList()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT * FROM Assunto WHERE StatusReg = 1";
                    var assunto = await connection.QueryAsync<AssuntoEntity>(query);
                    return assunto.ToList();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro de banco de dados: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o assunto: " + ex.Message, ex);
            }
        }


        public async Task<string> UpdateAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  UPDATE Assunto 
                                            SET Descricao = @Descricao, 
                                                UltimaAtualizacao = @UltimaAtualizacao 
                                            WHERE CodAs = @CodAs";

                            var parameters = new
                            {
                                assuntoEntity.Descricao,
                                UltimaAtualizacao = DateTime.Now,
                                assuntoEntity.CodAs
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Assunto atualizado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao atualizar assunto";
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<string> DeleteAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  UPDATE Assunto 
                                            SET StatusReg = 0, 
                                                UltimaAtualizacao = @UltimaAtualizacao 
                                            WHERE CodAs = @CodAs";

                            var parameters = new
                            {
                                UltimaAtualizacao = DateTime.Now,
                                assuntoEntity.CodAs
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Assunto deletado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao deletar assunto";
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<int> CreateAutor(AutorEntity autorEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  INSERT INTO Autor (Nome, StatusReg, DataCriacao, UltimaAtualizacao) 
                                            VALUES (@Nome, @StatusReg, @DataCriacao, @UltimaAtualizacao);
                                            SELECT CAST(SCOPE_IDENTITY() as int);";

                            var parameters = new
                            {
                                autorEntity.Nome,
                                StatusReg = 1,
                                DataCriacao = DateTime.Now,
                                UltimaAtualizacao = DateTime.Now
                            };

                            var autorId = await connection.QuerySingleAsync<int>(query, parameters, transaction: transaction);

                            transaction.Commit();

                            return autorId;
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar iniciar a transação: " + ex.Message);
            }
        }

        public async Task<AutorEntity> GetAutor(AutorEntity autorEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT * FROM Autor WHERE CodAu = @CodAu AND StatusReg = 1";
                    var autor = await connection.QueryFirstOrDefaultAsync<AutorEntity>(query, new { CodAu = autorEntity.CodAu });
                    return autor;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro de banco de dados: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o autor: " + ex.Message, ex);
            }
        }

        public async Task<List<AutorEntity>> GetAutorList()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT * FROM Autor WHERE StatusReg = 1";
                    var autor = await connection.QueryAsync<AutorEntity>(query);
                    return autor.ToList();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro de banco de dados: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o autor: " + ex.Message, ex);
            }
        }


        public async Task<string> UpdateAutor(AutorEntity autorEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  UPDATE Autor 
                                            SET Nome = @Nome, 
                                                UltimaAtualizacao = @UltimaAtualizacao 
                                            WHERE CodAu = @CodAu";

                            var parameters = new
                            {
                                autorEntity.Nome,
                                UltimaAtualizacao = DateTime.Now,
                                autorEntity.CodAu
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Autor atualizado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao atualizar autor";
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<string> DeleteAutor(AutorEntity autorEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"
                        UPDATE Autor 
                        SET StatusReg = 0, 
                            UltimaAtualizacao = @UltimaAtualizacao 
                        WHERE CodAu = @CodAu";

                            var parameters = new
                            {
                                UltimaAtualizacao = DateTime.Now,
                                autorEntity.CodAu
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Autor deletado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao deletar autor";
                            }
                        }
                        catch (SqlException ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<int> CreateTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @" INSERT INTO TipoVenda (Nome, StatusReg, DataCriacao, UltimaAtualizacao) 
                                           VALUES (@Nome, @StatusReg, @DataCriacao, @UltimaAtualizacao);
                                           SELECT CAST(SCOPE_IDENTITY() as int);";

                            var parameters = new
                            {
                                tipoVendaEntity.Nome,
                                StatusReg = 1,
                                DataCriacao = DateTime.Now,
                                UltimaAtualizacao = DateTime.Now
                            };

                            var tipoVendaId = await connection.QuerySingleAsync<int>(query, parameters, transaction: transaction);

                            transaction.Commit();

                            return tipoVendaId;
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar iniciar a transação: " + ex.Message);
            }
        }

        public async Task<TipoVendaEntity> GetTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT * FROM Tipo_Venda WHERE Codv = @Codv";
                    var tipoVenda = await connection.QueryFirstOrDefaultAsync<TipoVendaEntity>(query, new { Codv = tipoVendaEntity.Codv });
                    return tipoVenda;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro de banco de dados: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o tipo de venda: " + ex.Message, ex);
            }
        }


        public async Task<string> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  UPDATE TipoVenda 
                                            SET Nome = @Nome, 
                                                UltimaAtualizacao = @UltimaAtualizacao 
                                            WHERE Codv = @Codv";

                            var parameters = new
                            {
                                tipoVendaEntity.Nome,
                                UltimaAtualizacao = DateTime.Now,
                                tipoVendaEntity.Codv
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Tipo de venda atualizado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao atualizar tipo de venda";
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<string> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @" UPDATE TipoVenda 
                                           SET StatusReg = 0, 
                                               UltimaAtualizacao = @UltimaAtualizacao 
                                           WHERE Codv = @Codv";

                            var parameters = new
                            {
                                UltimaAtualizacao = DateTime.Now,
                                tipoVendaEntity.Codv
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Tipo de venda deletado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao deletar tipo de venda";
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }


        public async Task<int> CreateLivroValor(LivroValorEntity livroValorEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"
                        INSERT INTO Livro_Valor (Livro_Codl, Venda_Codv, Valor_Venda, StatusReg, DataCriacao, UltimaAtualizacao) 
                        VALUES (@Livro_Codl, @Venda_Codv, @Valor_Venda, @StatusReg, @DataCriacao, @UltimaAtualizacao);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

                            var parameters = new
                            {
                                livroValorEntity.Livro_Codl,
                                livroValorEntity.Venda_Codv,
                                livroValorEntity.Valor_venda,
                                StatusReg = 1,
                                DataCriacao = DateTime.Now,
                                UltimaAtualizacao = DateTime.Now
                            };

                            var livroValorId = await connection.QuerySingleAsync<int>(query, parameters, transaction: transaction);


                            transaction.Commit();

                            return livroValorId;
                        }
                        catch (SqlException ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar iniciar a transação: " + ex.Message);
            }
        }

        public async Task<LivroValorEntity> GetLivroValor(int livroCodl, int vendaCodv)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"  SELECT Livro_Codl, Venda_Codv, Valor_Venda, StatusReg 
                                    FROM Livro_Valor 
                                    WHERE Livro_Codl = @Livro_Codl AND Venda_Codv = @Venda_Codv AND StatusReg = 1";

                    var livroValor = await connection.QuerySingleOrDefaultAsync<LivroValorEntity>(query, new { Livro_Codl = livroCodl, Venda_Codv = vendaCodv });

                    return livroValor;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro de banco de dados: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex.Message);
            }
        }

        public async Task<string> UpdateLivroValor(LivroValorEntity livroValorEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"
                        UPDATE Livro_Valor 
                        SET Valor_Venda = @Valor_Venda, 
                            UltimaAtualizacao = @UltimaAtualizacao 
                        WHERE Livro_Codl = @Livro_Codl AND Venda_Codv = @Venda_Codv";

                            var parameters = new
                            {
                                livroValorEntity.Valor_venda,
                                UltimaAtualizacao = DateTime.Now,
                                livroValorEntity.Livro_Codl,
                                livroValorEntity.Venda_Codv
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                           if (result > 0)
                            {
                                transaction.Commit();
                                return "Livro_Valor atualizado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao atualizar Livro_Valor";
                            }
                        }
                        catch (SqlException ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<string> DeleteLivroValor(int livroCodl, int vendaCodv)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  UPDATE Livro_Valor 
                                            SET StatusReg = 0, 
                                                UltimaAtualizacao = @UltimaAtualizacao 
                                            WHERE Livro_Codl = @Livro_Codl AND Venda_Codv = @Venda_Codv";

                            var parameters = new
                            {
                                UltimaAtualizacao = DateTime.Now,
                                Livro_Codl = livroCodl,
                                Venda_Codv = vendaCodv
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Livro_Valor deletado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao deletar Livro_Valor";
                            }
                        }
                        catch (SqlException ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<int> CreateLivroAutor(LivroAutorEntity livroAutorEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  INSERT INTO Livro_Autor (Livro_Codl, Autor_CodAu, StatusReg, DataCriacao, UltimaAtualizacao) 
                                            VALUES (@LivroCodl, @AutorCodAu, @StatusReg, @DataCriacao, @UltimaAtualizacao);";

                            var parameters = new
                            {
                                livroAutorEntity.LivroCodl,
                                livroAutorEntity.AutorCodAu,
                                StatusReg = 1,
                                DataCriacao = DateTime.Now,
                                UltimaAtualizacao = DateTime.Now
                            };

                            var livroAutorId = await connection.QueryFirstOrDefaultAsync<int>(query, parameters, transaction: transaction);


                            transaction.Commit();

                            return livroAutorId;
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar iniciar a transação: " + ex.Message);
            }
        }

        public async Task<IEnumerable<LivroAutorEntity>> GetLivroAutor(int livroCodl)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = @"  SELECT Livro_Codl as LivroCodl, Autor_CodAu as AutorCodAu, StatusReg
                                      FROM Livro_Autor
                                     WHERE Livro_Codl = @LivroCodl
                                       AND StatusReg = 1";

                    var livroAutores = await connection.QueryAsync<LivroAutorEntity>(query, new { LivroCodl = livroCodl });
                    return livroAutores;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar os autores do livro com Codl: {LivroCodl}", livroCodl);
                throw new Exception("Erro ao acessar o banco de dados.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro desconhecido ao buscar os autores do livro com Codl: {LivroCodl}", livroCodl);
                throw new Exception("Erro ao processar a requisição.", ex);
            }
        }



        public async Task<string> DeleteLivroAutor(int livroCodl, int autorCodAu)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  UPDATE Livro_Autor 
                                            SET StatusReg = 0, 
                                                UltimaAtualizacao = @UltimaAtualizacao 
                                            WHERE Livro_Codl = @Livro_Codl AND Autor_CodAu = @AutorCodAu";

                            var parameters = new
                            {
                                UltimaAtualizacao = DateTime.Now,
                                Livro_Codl = livroCodl,
                                AutorCodAu = autorCodAu
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Livro_Autor deletado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao deletar Livro_Autor";
                            }
                        }
                        catch (SqlException ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {

                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }

        public async Task<int> CreateLivroAssunto(LivroAssuntoEntity livroAssuntoEntity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"  INSERT INTO Livro_Assunto (Livro_Codl, Assunto_CodAs, StatusReg, DataCriacao, UltimaAtualizacao) 
                                            VALUES (@LivroCodl, @AssuntoCodAs, @StatusReg, @DataCriacao, @UltimaAtualizacao);";

                            var parameters = new
                            {
                                livroAssuntoEntity.LivroCodl,
                                livroAssuntoEntity.AssuntoCodAs,
                                StatusReg = 1,
                                DataCriacao = DateTime.Now,
                                UltimaAtualizacao = DateTime.Now
                            };

                            var livroAssuntoId = await connection.QueryFirstOrDefaultAsync<int>(query, parameters, transaction: transaction);

                            transaction.Commit();

                            return livroAssuntoId;
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar iniciar a transação: " + ex.Message);
            }
        }

        public async Task<LivroAssuntoEntity> GetLivroAssunto(int livroCodl)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = @"
                                    SELECT Livro_Codl as LivroCodl, Assunto_CodAs as AssuntoCodAs, StatusReg
                                      FROM Livro_Assunto
                                     WHERE Livro_Codl = @LivroCodl 
                                       AND StatusReg = 1";

                    var livroAssuntos = await connection.QueryFirstOrDefaultAsync<LivroAssuntoEntity>(query, new { LivroCodl = livroCodl });
                    return livroAssuntos;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar os assuntos do livro com Codl: {LivroCodl}", livroCodl);
                throw new Exception("Erro ao acessar o banco de dados.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro desconhecido ao buscar os assuntos do livro com Codl: {LivroCodl}", livroCodl);
                throw new Exception("Erro ao processar a requisição.", ex);
            }
        }


        public async Task<string> DeleteLivroAssunto(int livroCodl, int assuntoCodAs)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = @"
                                UPDATE Livro_Assunto 
                                SET StatusReg = 0, 
                                    UltimaAtualizacao = @UltimaAtualizacao 
                                WHERE Livro_Codl = @LivroCodl 
                                  AND Assunto_CodAs = @AssuntoCodAs";

                            var parameters = new
                            {
                                UltimaAtualizacao = DateTime.Now,
                                Livro_Codl = livroCodl,
                                Assunto_CodAs = assuntoCodAs
                            };

                            var result = await connection.ExecuteAsync(query, parameters, transaction: transaction);

                            if (result > 0)
                            {
                                transaction.Commit();
                                return "Livro_Assunto deletado com sucesso";
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Erro ao deletar Livro_Assunto";
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro de banco de dados: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro ao tentar iniciar a transação: " + ex.Message;
            }
        }


        public async Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT 1 as tipoRel, Autor, Livro, Assunto FROM vw_LivrosPorAutorComAssunto;";

                    var result = await connection.QueryAsync<RelLivrosPorAutorComAssuntoDTO>(query);
                    return result.ToList();
                }
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public async Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "select 2 as tipoRel, Autor, Livro, Assunto, Valor, TipoVenda from vw_LivrosPorAutorComValorETipoVenda";


                    var result = await connection.QueryAsync<RelLivrosPorAutorComValorETipoVendaDTO>(query);
                    return result.ToList();
                }
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }


        }


    }
}
