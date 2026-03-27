using Microsoft.AspNetCore.Mvc;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _service;

        public ClientesController(ClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Listar()
        {
            var clientes = await _service.ListarAsync();
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
            var novoCliente = await _service.CriarAsync(cliente);
            return CreatedAtAction(nameof(BuscarPorId), new { id = novoCliente.Id }, novoCliente);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Cliente cliente)
        {
            var atualizado = await _service.AtualizarAsync(id, cliente);

            if (!atualizado)
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