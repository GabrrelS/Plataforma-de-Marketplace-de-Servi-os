using Microsoft.AspNetCore.Mvc;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropostasController : ControllerBase
    {
        private readonly PropostaService _service;

        public PropostasController(PropostaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Proposta>>> Listar()
        {
            var propostas = await _service.ListarAsync();
            return Ok(propostas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Proposta>> BuscarPorId(int id)
        {
            var proposta = await _service.BuscarPorIdAsync(id);

            if (proposta == null)
                return NotFound();

            return Ok(proposta);
        }

        [HttpPost]
        public async Task<ActionResult<Proposta>> Criar(Proposta proposta)
        {
            var novaProposta = await _service.CriarAsync(proposta);
            return CreatedAtAction(nameof(BuscarPorId), new { id = novaProposta.Id }, novaProposta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Proposta proposta)
        {
            var atualizado = await _service.AtualizarAsync(id, proposta);

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