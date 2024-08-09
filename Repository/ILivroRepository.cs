using Domain.Dtos;
using Domain.Entity;

namespace Repository
{
    public interface ILivroRepository
    {
        Task<int> CreateLivro(LivroEntity livroEntity);
        Task<LivroEntity> GetLivro(LivroEntity param);
        Task<string> UpdateLivro(LivroEntity param);
        Task<string> DeleteLivro(LivroEntity param);

        Task<int> CreateAssunto(AssuntoEntity param);
        Task<AssuntoEntity> GetAssunto(AssuntoEntity assuntoEntity);
        Task<string> UpdateAssunto(AssuntoEntity param);
        Task<string> DeleteAssunto(AssuntoEntity param);

        Task<int> CreateAutor(AutorEntity param);
        Task<AutorEntity> GetAutor(AutorEntity param);
        Task<string> UpdateAutor(AutorEntity param);
        Task<string> DeleteAutor(AutorEntity param);

        Task<int> CreateTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<TipoVendaEntity> GetTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<string> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<string> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity);

        Task<int> CreateLivroValor(LivroValorEntity livroValorEntity);
        Task<LivroValorEntity> GetLivroValor(int livroCodl, int vendaCodv);
        Task<string> UpdateLivroValor(LivroValorEntity livroValorEntity);
        Task<string> DeleteLivroValor(int livroCodl, int vendaCodv);
        
        Task<int> CreateLivroAutor(LivroAutorEntity livroAutorEntity);
        Task<IEnumerable<LivroAutorEntity>> GetLivroAutor(int livroCodl);
        Task<string> DeleteLivroAutor(int livroCodl, int autorCodAu);
        
        Task<int> CreateLivroAssunto(LivroAssuntoEntity livroAssuntoEntity);
        Task<LivroAssuntoEntity> GetLivroAssunto(int livroCodl);
        Task<string> DeleteLivroAssunto(int livroCodl, int assuntoCodAs);


        Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio);

        Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio);

    }
}
