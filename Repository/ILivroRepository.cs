using Domain.Dtos;
using Domain.Entity;
using System.Data;

namespace Repository
{
    public interface ILivroRepository
    {
        Task<int> CreateLivro(LivroEntity livroEntity, IDbTransaction transaction);
        Task<LivroEntity> GetLivro(LivroEntity param);
        Task<List<LivroEntity>> GetLivroList();
        Task<bool> UpdateLivro(LivroEntity param, IDbTransaction transaction);
        Task<bool> DeleteLivro(LivroEntity param, IDbTransaction transaction);


        Task<int> CreateAssunto(AssuntoEntity param, IDbTransaction transaction);
        Task<AssuntoEntity> GetAssunto(AssuntoEntity param);
        Task<List<AssuntoEntity>> GetAssuntoList();
        Task<bool> UpdateAssunto(AssuntoEntity param, IDbTransaction transaction);
        Task<bool> DeleteAssunto(AssuntoEntity param, IDbTransaction transaction);


        Task<int> CreateAutor(AutorEntity param, IDbTransaction transaction);
        Task<AutorEntity> GetAutor(AutorEntity param);
        Task<List<AutorEntity>> GetAutorList();
        Task<bool> UpdateAutor(AutorEntity param, IDbTransaction transaction);
        Task<bool> DeleteAutor(AutorEntity param, IDbTransaction transaction);


        Task<int> CreateTipoVenda(TipoVendaEntity tipoVendaEntity, IDbTransaction transaction);
        Task<TipoVendaEntity> GetTipoVenda(TipoVendaEntity tipoVendaEntity);
        Task<List<TipoVendaEntity>> GetTipoVendaList();
        Task<bool> UpdateTipoVenda(TipoVendaEntity tipoVendaEntity, IDbTransaction transaction);
        Task<bool> DeleteTipoVenda(TipoVendaEntity tipoVendaEntity, IDbTransaction transaction);


        Task<bool> CreateLivroValor(LivroValorEntity livroValorEntity, IDbTransaction transaction);
        Task<List<LivroValorEntity>> GetLivroValor(int livroCodl);
        Task<List<LivroValorEntity>> GetLivroValorList();
        Task<bool> UpdateLivroValor(LivroValorEntity livroValorEntity, IDbTransaction transaction);
        Task<bool> DeleteLivroValor(int livroCodl, int vendaCodv, IDbTransaction transaction);


        Task<int> CreateLivroAutor(LivroAutorEntity livroAutorEntity, IDbTransaction transaction);
        Task<IEnumerable<LivroAutorEntity>> GetLivroAutor(int livroCodl);
        Task<IEnumerable<LivroAutorEntity>> GetLivroAutor(int livroCodl, IDbTransaction transaction = null);
        Task<bool> DeleteLivroAutor(int livroCodl, int autorCodAu, IDbTransaction transaction);


        Task<int> CreateLivroAssunto(LivroAssuntoEntity livroAssuntoEntity, IDbTransaction transaction);
        Task<LivroAssuntoEntity> GetLivroAssunto(int livroCodl);
        Task<LivroAssuntoEntity> GetLivroAssunto(int livroCodl, IDbTransaction transaction = null);
        Task<bool> DeleteLivroAssunto(int livroCodl, int assuntoCodAs, IDbTransaction transaction);


        Task<List<RelLivrosPorAutorComValorETipoVendaDTO>> GerarRelatorioPorAutorComValor(int tipoRelatorio);
        Task<List<RelLivrosPorAutorComAssuntoDTO>> GerarRelatorioPorAutorComAssunto(int tipoRelatorio);
    }
}
