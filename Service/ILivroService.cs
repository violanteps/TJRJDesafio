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


        Task<bool> CreateTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<TipoVendaEntity> GetTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<TipoVendaEntity> GetTipoVendaList(TipoVendaEntity tipoVendaEntity);
        Task<bool> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<bool> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity);


        Task<bool> CreateLivroValor(LivroValorEntity livroValorEntity);
        Task<LivroValorEntity> GetLivroValor(int livroCodl);
        Task<List<LivroValorEntity>> GetLivroValorList();
        Task<bool> UpdateLivroValor(LivroValorEntity livroValorEntity);
        Task<bool> DeleteLivroValor(int livroCodl, int vendaCodv);


        Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio);
        Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio);




    }
}
