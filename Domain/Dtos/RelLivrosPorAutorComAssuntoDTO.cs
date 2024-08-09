namespace Domain.Dtos
{
    public class RelLivrosPorAutorComAssuntoDTO
    {

        public int tipoRel { get; set; }

        public string Autor { get; set; }

        public string Livro { get; set; }

        public string Assunto { get; set; }

        public RelLivrosPorAutorComAssuntoDTO() { }
    }
}
