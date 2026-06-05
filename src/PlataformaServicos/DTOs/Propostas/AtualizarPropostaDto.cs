using System.ComponentModel.DataAnnotations;

namespace PlataformaServicos.DTOs
{
    public class AtualizarPropostaDto
    {
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Range(1, 1000000)]
        public decimal Valor { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}