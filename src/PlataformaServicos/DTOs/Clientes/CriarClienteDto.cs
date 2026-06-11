using System.ComponentModel.DataAnnotations;

namespace PlataformaServicos.DTOs.Clientes
{
    public class CriarClienteDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;
    }
}
