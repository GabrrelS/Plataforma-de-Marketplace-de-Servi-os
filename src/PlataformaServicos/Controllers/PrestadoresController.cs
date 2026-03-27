using Microsoft.AspNetCore.Mvc;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestadoresController : ControllerBase
    {
        private readonly PrestadorService _service;

        public PrestadoresController(PrestadorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Prestador>>> Listar()
        {
            var prestadores = await _service.ListarAsync();
            return Ok(prestadores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prestador>> BuscarPorId(int id)
        {
            var prestador = await _service.BuscarPorIdAsync(id);

            if (prestador == null)
                return NotFound();

            return Ok(prestador);
        }

        [HttpPost]
        public async Task<ActionResult<Prestador>> Criar(Prestador prestador)
        {
            var novoPrestador = await _service.CriarAsync(prestador);
            return CreatedAtAction(nameof(BuscarPorId), new { id = novoPrestador.Id }, novoPrestador);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Prestador prestador)
        {
            var atualizado = await _service.AtualizarAsync(id, prestador);

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