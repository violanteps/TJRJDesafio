using Domain.Dtos;
using Domain.Entity;

namespace Service
{
    public interface ILivroService
    {
        Task<bool> CreateLivro(LivroEntity param);
        Task<LivroEntity> GetLivro(LivroEntity param);
        Task<List<LivroEntity>> GetLivroList();
        Task<bool> UpdateLivro(LivroEntity param);
        Task<bool> DeleteLivro(LivroEntity param);

        Task<bool> CreateAssunto(AssuntoEntity param);
        Task<AssuntoEntity> GetAssunto(AssuntoEntity param);
        Task<List<AssuntoEntity>> GetAssuntoList();
        Task<bool> UpdateAssunto(AssuntoEntity param);
        Task<bool> DeleteAssunto(AssuntoEntity param);

        Task<bool> CreateAutor(AutorEntity param);
        Task<AutorEntity> GetAutor(AutorEntity param);
        Task<List<AutorEntity>> GetAutorList();
        Task<bool> UpdateAutor(AutorEntity param);
        Task<bool> DeleteAutor(AutorEntity param);

        Task<bool> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<bool> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<TipoVendaEntity> GetTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<bool> CreateTipoVenda(TipoVendaEntity tipoVendaEntity);

        Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio);
        Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio);




    }
}
