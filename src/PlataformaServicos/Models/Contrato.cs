namespace PlataformaServicos.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Ativo";

        public int ClienteId { get; set; }
        public int PrestadorId { get; set; }
        public int PropostaId { get; set; }
    }
}