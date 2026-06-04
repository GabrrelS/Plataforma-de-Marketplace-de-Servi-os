using System.ComponentModel.DataAnnotations;

public class Proposta
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Descricao { get; set; } = string.Empty;

    [Range(1, 1000000)]
    public decimal Valor { get; set; }

    public string Status { get; set; } = "Pendente";

    public int ClienteId { get; set; }

    public int PrestadorId { get; set; }
}