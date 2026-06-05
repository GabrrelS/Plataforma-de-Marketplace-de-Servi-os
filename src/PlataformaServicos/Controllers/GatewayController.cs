using Microsoft.AspNetCore.Mvc;
using PlataformaServicos.HttpClients;

namespace PlataformaServicos.Controllers;

[ApiController]
[Route("api/gateway")]
public class GatewayController : ControllerBase
{
    private readonly GatewayService _gateway;
    private readonly ILogger<GatewayController> _logger;

    public GatewayController(GatewayService gateway, ILogger<GatewayController> logger)
    {
        _gateway = gateway;
        _logger  = logger;
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CriarPedido([FromBody] object pedido)
    {
        _logger.LogInformation("Gateway recebeu pedido para OrderService");
        var sucesso = await _gateway.CriarPedidoAsync(pedido);
        if (sucesso)
            return Ok(new { message = "Pedido enviado ao OrderService com sucesso" });
        return StatusCode(503, new { message = "OrderService temporariamente indisponível." });
    }

    [HttpGet("products")]
    public async Task<IActionResult> ListarProdutos()
    {
        _logger.LogInformation("Gateway consultando ProductService");
        var produtos = await _gateway.ListarProdutosAsync();
        if (produtos != null)
            return Ok(produtos);
        return StatusCode(503, new { message = "ProductService temporariamente indisponível." });
    }

    [HttpPost("products")]
    public async Task<IActionResult> CriarProduto([FromBody] object produto)
    {
        var sucesso = await _gateway.CriarProdutoAsync(produto);
        if (sucesso)
            return Ok(new { message = "Produto criado no ProductService" });
        return StatusCode(503, new { message = "ProductService indisponível." });
    }

    [HttpPost("notificacoes/proposta/{id}/status")]
    public async Task<IActionResult> NotificarStatusProposta(int id, [FromBody] StatusDto dto)
    {
        var sucesso = await _gateway.EnviarNotificacaoAsync(id, dto.Status);
        if (sucesso)
            return Ok(new { message = $"Notificação enviada para proposta {id}" });
        return StatusCode(503, new { message = "NotificationService indisponível." });
    }
}

public record StatusDto(string Status);