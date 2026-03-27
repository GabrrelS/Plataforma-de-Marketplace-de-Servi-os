namespace PlataformaServicos.Models
{
    public class Proposta
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Status { get; set; } = "Pendente";

        public int ClienteId { get; set; }
        public int PrestadorId { get; set; }
    }
}