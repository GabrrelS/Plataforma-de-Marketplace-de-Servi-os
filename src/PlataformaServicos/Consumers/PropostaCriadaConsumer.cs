using MassTransit;
using PlataformaServicos.Events;

namespace PlataformaServicos.Consumers;

public class PropostaCriadaConsumer : IConsumer<PropostaCriadaEvent>
{
    private readonly ILogger<PropostaCriadaConsumer> _logger;

    public PropostaCriadaConsumer(ILogger<PropostaCriadaConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PropostaCriadaEvent> context)
    {
        var evento = context.Message;
        _logger.LogInformation(
            "Proposta recebida via RabbitMQ — Id: {Id}, Título: {Titulo}, Valor: {Valor}",
            evento.PropostaId, evento.Titulo, evento.Valor
        );
        await Task.CompletedTask;
    }
}