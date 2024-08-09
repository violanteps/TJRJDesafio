using Domain.Dtos;
using Domain.Entity;
using Repository;
using System;
using System.Threading.Tasks;

namespace Service
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }

        public async Task<string> CreateLivro(LivroEntity livroEntity)
        {
            try
            {
                if (livroEntity.LivroAutores == null || livroEntity.LivroAutores.Count == 0)
                    return $"Erro ao criar livro. Deve existir ao menos 1 autor";
                //E preciso que exista o Autor
                //var result = _livroRepository.GetAutor(autorEntity);
                //E preciso que exista o Assunto

                var result = await _livroRepository.CreateLivro(livroEntity);

                var result2 = 1;

                return "OK";
            }
            catch (Exception ex)
            {

                return $"Erro ao criar livro: {ex.Message}";
            }
        }

        public async Task<string> GetLivro(LivroEntity livroEntity)
        {
            try
            {

                var result = _livroRepository.GetLivro(livroEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao obter livro: {ex.Message}";
            }
        }

        public async Task<string> UpdateLivro(LivroEntity livroEntity)
        {
            try
            {
                //Atualizar tb a Livro_Autor
                //Atualizar a livro assunto
                //Atualizar a livro valor
                var result = _livroRepository.UpdateLivro(livroEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao atualizar livro: {ex.Message}";
            }
        }

        public async Task<string> DeleteLivro(LivroEntity livroEntity)
        {
            try
            {
                //É feita exclusão lógica
                var result = _livroRepository.DeleteLivro(livroEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao deletar livro: {ex.Message}";
            }
        }

        public async Task<string> CreateAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {
                var result = await _livroRepository.CreateAssunto(assuntoEntity);
                return "";
            }
            catch (Exception ex)
            {
                return $"Erro ao criar assunto: {ex.Message}";
            }
        }

        public async Task<string> GetAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {
                var result = _livroRepository.GetAssunto(assuntoEntity);
                return await result;
            }
            catch (Exception ex)
            {
                return $"Erro ao obter assunto: {ex.Message}";
            }
        }

        public async Task<string> UpdateAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {

                //Atualizar a livro assunto

                var result = _livroRepository.UpdateAssunto(assuntoEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao atualizar assunto: {ex.Message}";
            }
        }

        public async Task<string> DeleteAssunto(AssuntoEntity assuntoEntity)
        {
            try
            {

                //Atualizar a livro assunto

                var result = _livroRepository.DeleteAssunto(assuntoEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao deletar assunto: {ex.Message}";
            }
        }

        public async Task<string> CreateAutor(AutorEntity autorEntity)
        {
            try
            {
                var result = await _livroRepository.CreateAutor(autorEntity);
                return  "";
            }
            catch (Exception ex)
            {

                return $"Erro ao criar autor: {ex.Message}";
            }
        }

        public async Task<string> GetAutor(AutorEntity autorEntity)
        {
            try
            {
                var result = _livroRepository.GetAutor(autorEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao obter autor: {ex.Message}";
            }
        }

        public async Task<string> UpdateAutor(AutorEntity autorEntity)
        {
            try
            {
                //Atualizar tb a Livro_Autor

                var result = _livroRepository.UpdateAutor(autorEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao atualizar autor: {ex.Message}";
            }
        }

        public async Task<string> DeleteAutor(AutorEntity autorEntity)
        {
            try
            {
                //Atualizar tb a Livro_Autor

                var result = _livroRepository.DeleteAutor(autorEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao deletar autor: {ex.Message}";
            }
        }

        public async Task<string> CreateTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {
                var result = await _livroRepository.CreateTipoVenda(tipoVendaEntity);
                return "";
            }
            catch (Exception ex)
            {

                return $"Erro ao criar autor: {ex.Message}";
            }
        }

        public async Task<string> GetTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {

                var result = _livroRepository.GetTipoVenda(tipoVendaEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao obter autor: {ex.Message}";
            }
        }

        public async Task<string> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {
                //Atualizar a livro_valor
                var result = _livroRepository.UpdateTipoVenda(tipoVendaEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao atualizar autor: {ex.Message}";
            }
        }

        public async Task<string> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity)
        {
            try
            {
                //Atualizo Livro valor
                var result = _livroRepository.DeleteTipoVenda(tipoVendaEntity);
                return await result;
            }
            catch (Exception ex)
            {

                return $"Erro ao deletar autor: {ex.Message}";
            }
        }


        public async Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio)
        {
            try
            {
                var result = await _livroRepository.GerarRelatorioPorAutorComAssunto(tipoRelatorio);
                return result;

            }
            catch (Exception ex)
            {

                throw new InvalidOperationException($"Erro ao gerar o relatório.", ex);
            }
        }


        public async Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio)
        {
            try
            {
                var result = await _livroRepository.GerarRelatorioPorAutorComValor(tipoRelatorio);
                return result;

            }
            catch (Exception ex)
            {

                throw new InvalidOperationException($"Erro ao gerar o relatório.", ex);
            }
        }

    }
}
