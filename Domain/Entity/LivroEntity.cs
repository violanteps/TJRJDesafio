namespace Domain.Entity
{
    public class LivroEntity
    {
        public int Codl { get; set; }

        public string Titulo { get; set; }

        public string Editora { get; set; }

        public int Edicao { get; set; }

        public string AnoPublicacao { get; set; }

        public int Status { get; set; }

        public LivroAssuntoEntity LivroAssuntoEntity { get; set; }

        public List<LivroAutorEntity> LivroAutores { get; set; }

    }
}
