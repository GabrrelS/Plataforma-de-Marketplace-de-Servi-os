using System.ComponentModel.DataAnnotations;

namespace PlataformaServicos.DTOs.Prestadores
{
    public class CriarPrestadorDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        [Range(0, 5)]
        public decimal NotaMedia { get; set; }
    }
}
