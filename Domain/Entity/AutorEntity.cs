namespace Domain.Entity
{
    public class AutorEntity
    {
        public int CodAu { get; set; }

        public string Nome { get; set; }

        public List<LivroAutorEntity> LivroAutor { get; set; }
    }
}
