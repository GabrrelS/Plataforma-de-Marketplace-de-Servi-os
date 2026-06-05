using System.Text;
using System.Text.Json;

namespace PlataformaServicos.HttpClients;

public class GatewayService
{
    private readonly IHttpClientFactory _factory;
    private readonly ILogger<GatewayService> _logger;

    public GatewayService(IHttpClientFactory factory, ILogger<GatewayService> logger)
    {
        _factory = factory;
        _logger  = logger;
    }

    public async Task<bool> CriarPedidoAsync(object pedido)
    {
        try
        {
            var client  = _factory.CreateClient("OrderService");
            var json    = JsonSerializer.Serialize(pedido);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/orders", content);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Pedido enviado ao OrderService com sucesso");
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OrderService indisponível.");
            return false;
        }
    }

    public async Task<string?> ListarProdutosAsync()
    {
        try
        {
            var client   = _factory.CreateClient("ProductService");
            var response = await client.GetAsync("/products");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProductService indisponível.");
            return null;
        }
    }

    public async Task<bool> CriarProdutoAsync(object produto)
    {
        try
        {
            var client  = _factory.CreateClient("ProductService");
            var json    = JsonSerializer.Serialize(produto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/products", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto.");
            return false;
        }
    }

    public async Task<bool> EnviarNotificacaoAsync(int propostaId, string status)
    {
        try
        {
            var client  = _factory.CreateClient("NotificationService");
            var payload = new { PropostaId = propostaId, Status = status };
            var json    = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/notificacao/proposta-status", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "NotificationService indisponível.");
            return false;
        }
    }
}