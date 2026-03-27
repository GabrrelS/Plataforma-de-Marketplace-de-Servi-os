namespace PlataformaServicos.Models
{
    public class Prestador
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public decimal NotaMedia { get; set; }
    }
}