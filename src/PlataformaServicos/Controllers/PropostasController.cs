using Microsoft.AspNetCore.Mvc;
using MassTransit;
using PlataformaServicos.Events;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostasController : ControllerBase
{
    private readonly PropostaService _service;
    private readonly IPublishEndpoint _publishEndpoint;

    public PropostasController(PropostaService service, IPublishEndpoint publishEndpoint)
    {
        _service         = service;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<Proposta>>> Listar()
    {
        return Ok(await _service.ListarAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Proposta>> BuscarPorId(int id)
    {
        var proposta = await _service.BuscarPorIdAsync(id);
        if (proposta == null) return NotFound();
        return Ok(proposta);
    }

    [HttpPost]
    public async Task<ActionResult<Proposta>> Criar(Proposta proposta)
    {
        var novaProposta = await _service.CriarAsync(proposta);

        // Publica evento no RabbitMQ via MassTransit
        await _publishEndpoint.Publish(new PropostaCriadaEvent
        {
            PropostaId  = novaProposta.Id,
            ClienteId   = novaProposta.ClienteId,
            PrestadorId = novaProposta.PrestadorId,
            Titulo      = novaProposta.Titulo,
            Valor       = novaProposta.Valor
        });

        return CreatedAtAction(nameof(BuscarPorId), new { id = novaProposta.Id }, novaProposta);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, Proposta proposta)
    {
        var atualizado = await _service.AtualizarAsync(id, proposta);
        if (!atualizado) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Deletar(int id)
    {
        var deletado = await _service.DeletarAsync(id);
        if (!deletado) return NotFound();
        return NoContent();
    }
}