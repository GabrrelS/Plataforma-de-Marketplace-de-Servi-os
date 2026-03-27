namespace PlataformaServicos.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; } = string.Empty;

        public int ContratoId { get; set; }
    }
}
