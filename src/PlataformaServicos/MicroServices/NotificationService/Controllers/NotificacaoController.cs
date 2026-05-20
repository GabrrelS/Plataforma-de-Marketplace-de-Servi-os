using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/notificacoes")]
public class NotificacaoController : ControllerBase
{
    private readonly NotificacaoService _service;

    public NotificacaoController(
        NotificacaoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Enviar(
        [FromBody] PropostaStatusDto dto)
    {
        await _service.EnviarStatus(dto);

        return Ok(new
        {
            mensagem = "Notificação enviada"
        });
    }
}