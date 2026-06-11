using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlataformaServicos.CQRS.Clientes.Commands;
using PlataformaServicos.CQRS.Clientes.Queries;
using PlataformaServicos.DTOs.Clientes;
using PlataformaServicos.Models;

namespace PlataformaServicos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClientesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Listar()
            => Ok(await _mediator.Send(new ListarClientesQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> BuscarPorId(int id)
        {
            var cliente = await _mediator.Send(new BuscarClientePorIdQuery(id));
            return cliente == null ? NotFound() : Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Criar(CriarClienteDto dto)
        {
            var novo = await _mediator.Send(new CriarClienteCommand(dto.Nome, dto.Email, dto.Telefone));
            return CreatedAtAction(nameof(BuscarPorId), new { id = novo.Id }, novo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, AtualizarClienteDto dto)
        {
            var atualizado = await _mediator.Send(new AtualizarClienteCommand(id, dto.Nome, dto.Email, dto.Telefone));
            return atualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var deletado = await _mediator.Send(new DeletarClienteCommand(id));
            return deletado ? NoContent() : NotFound();
        }
    }
}
