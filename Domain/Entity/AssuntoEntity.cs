namespace Domain.Entity
{
    public class AssuntoEntity
    {
        public int CodAs { get; set; }

        public string Descricao { get; set; }

        public List<LivroAssuntoEntity> LivroAssuntos { get; set; }

    }
}
