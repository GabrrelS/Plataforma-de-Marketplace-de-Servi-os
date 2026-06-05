using Microsoft.AspNetCore.Mvc;
using MediatR;
using PlataformaServicos.Models;
using PlataformaServicos.Services;
using PlataformaServicos.Features.Clientes.Commands;
using PlataformaServicos.Features.Clientes.Queries;

namespace PlataformaServicos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _service;
        private readonly IMediator _mediator;

        // Injetamos o Mediator para o CQRS e mantemos o service para o CRUD restante
        public ClientesController(ClienteService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Listar()
        {
            // Usando CQRS (Query) via MediatR
            var clientes = await _mediator.Send(new ListarClientesQuery());
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> BuscarPorId(int id)
        {
            var cliente = await _service.BuscarPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Criar(Cliente cliente)
        {
            // Usando CQRS (Command) via MediatR
            var novoCliente = await _mediator.Send(new CriarClienteCommand(cliente));
            return CreatedAtAction(nameof(BuscarPorId), new { id = novoCliente.Id }, novoCliente);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Cliente cliente)
        {
            var updated = await _service.AtualizarAsync(id, cliente);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var deletado = await _service.DeletarAsync(id);

            if (!deletado)
                return NotFound();

            return NoContent();
        }
    }
}