using Microsoft.AspNetCore.Mvc;
using PlataformaServicos.Models;
using PlataformaServicos.Services;
using PlataformaServicos.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace PlataformaServicos.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PropostasController : ControllerBase
    {
        private readonly PropostaService _service;

        public PropostasController(PropostaService service)
        {
            _service = service;
        }

        // Cliente, Prestador ou Admin podem visualizar
        [Authorize(Roles = "Cliente,Prestador,Admin")]
        [HttpGet]
        public async Task<ActionResult<List<Proposta>>> Listar(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            return Ok(await _service.ListarAsync(
                pagina,
                tamanhoPagina));
        }

        // Cliente, Prestador ou Admin podem visualizar
        [Authorize(Roles = "Cliente,Prestador,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Proposta>> BuscarPorId(int id)
        {
            var proposta = await _service.BuscarPorIdAsync(id);

            if (proposta == null)
                return NotFound();

            return Ok(proposta);
        }

        // Apenas Cliente pode criar propostas
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<ActionResult<Proposta>> Criar(CriarPropostaDto dto)
        {
            var proposta = new Proposta
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                ClienteId = dto.ClienteId,
                PrestadorId = dto.PrestadorId,
                Status = "Pendente"
            };

            var novaProposta = await _service.CriarAsync(proposta);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = novaProposta.Id },
                novaProposta);
        }

        // Cliente ou Admin podem atualizar
        [Authorize(Roles = "Cliente,Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(
            int id,
            AtualizarPropostaDto dto)
        {
            var proposta = new Proposta
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Status = dto.Status
            };

            var atualizado = await _service.AtualizarAsync(id, proposta);

            if (!atualizado)
                return NotFound();

            return NoContent();
        }

        // Apenas Admin pode excluir
        [Authorize(Roles = "Admin")]
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