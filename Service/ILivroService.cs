using Domain.Dtos;
using Domain.Entity;

namespace Service
{
    public interface ILivroService
    {
        Task<string> CreateLivro(LivroEntity param);
        Task<LivroEntity> GetLivro(LivroEntity param);
        Task<string> UpdateLivro(LivroEntity param);
        Task<string> DeleteLivro(LivroEntity param);

        Task<string> CreateAssunto(AssuntoEntity param);
        Task<string> GetAssunto(AssuntoEntity param);
        Task<string> UpdateAssunto(AssuntoEntity param);
        Task<string> DeleteAssunto(AssuntoEntity param);

        Task<string> CreateAutor(AutorEntity param);
        Task<string> GetAutor(AutorEntity param);
        Task<string> UpdateAutor(AutorEntity param);
        Task<string> DeleteAutor(AutorEntity param);

        Task<string> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<string> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<string> GetTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<string> CreateTipoVenda(TipoVendaEntity tipoVendaEntity);

        Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio);
        Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio);




    }
}
