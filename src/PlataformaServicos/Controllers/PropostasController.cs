using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlataformaServicos.CQRS.Propostas.Commands;
using PlataformaServicos.CQRS.Propostas.Queries;
using PlataformaServicos.DTOs;
using PlataformaServicos.Models;

namespace PlataformaServicos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropostasController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PropostasController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<Proposta>>> Listar()
            => Ok(await _mediator.Send(new ListarPropostasQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<Proposta>> BuscarPorId(int id)
        {
            var proposta = await _mediator.Send(new BuscarPropostaPorIdQuery(id));
            return proposta == null ? NotFound() : Ok(proposta);
        }

        [HttpPost]
        public async Task<ActionResult<Proposta>> Criar(CriarPropostaDto dto)
        {
            var nova = await _mediator.Send(new CriarPropostaCommand(dto.Titulo, dto.Descricao, dto.Valor, dto.ClienteId, dto.PrestadorId));
            return CreatedAtAction(nameof(BuscarPorId), new { id = nova.Id }, nova);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, AtualizarPropostaDto dto)
        {
            var atualizado = await _mediator.Send(new AtualizarPropostaCommand(id, dto.Titulo, dto.Descricao, dto.Valor, dto.Status));
            return atualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var deletado = await _mediator.Send(new DeletarPropostaCommand(id));
            return deletado ? NoContent() : NotFound();
        }
    }
}
