using System.ComponentModel.DataAnnotations;

namespace Marketplace.API.DTOs.Propostas
{
    public class PropostaResponseDto
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public decimal Valor { get; set; }
    }
}