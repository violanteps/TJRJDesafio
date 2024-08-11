namespace Domain.Entity
{
    public class LivroEntity
    {
        public int Codl { get; set; }

        public string Titulo { get; set; }

        public string Editora { get; set; }

        public int Edicao { get; set; }

        public string AnoPublicacao { get; set; }

        public int StatusReg { get; set; }

        public DateTime DataCriacao { get; set; }
        
        public LivroAssuntoEntity LivroAssuntoEntity { get; set; }

        public List<LivroAutorEntity> LivroAutores { get; set; }
    }
}
