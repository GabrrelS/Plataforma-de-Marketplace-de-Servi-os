namespace NotificationService.Models;

public class PropostaStatusDto
{
    public Guid PropostaId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime AtualizadoEm { get; set; }
}