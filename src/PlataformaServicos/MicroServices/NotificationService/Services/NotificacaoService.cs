using NotificationService.Models;

namespace NotificationService.Services;

public class NotificacaoService
{
    // O seu amigo precisa colocar a lógica de envio aqui dentro, tipo:
    public async Task EnviarStatus(PropostaStatusDto dto)
    {
        // Lógica para enviar a notificação (Email, SMS, SignalR, etc.)
        await Task.CompletedTask;
    }
}