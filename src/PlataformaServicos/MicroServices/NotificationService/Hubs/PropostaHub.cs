using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Hubs;

public class PropostaHub : Hub
{
    public async Task EntrarGrupo(string propostaId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            $"proposta:{propostaId}");
    }
}