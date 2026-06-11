using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using NotificationService.Models;

namespace NotificationService.Services;

public class NotificacaoService
{
    private readonly IHubContext<PropostaHub> _hub;

    public NotificacaoService(IHubContext<PropostaHub> hub)
    {
        _hub = hub;
    }

    public async Task EnviarStatus(PropostaStatusDto dto)
    {
        // Envia para todos os clientes conectados no grupo da proposta
        await _hub.Clients
            .Group($"proposta:{dto.PropostaId}")
            .SendAsync("StatusAtualizado", new
            {
                propostaId   = dto.PropostaId,
                status       = dto.Status,
                atualizadoEm = dto.AtualizadoEm
            });
    }
}
