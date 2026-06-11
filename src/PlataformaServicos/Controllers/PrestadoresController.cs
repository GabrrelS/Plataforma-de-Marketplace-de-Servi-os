using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlataformaServicos.CQRS.Prestadores.Commands;
using PlataformaServicos.CQRS.Prestadores.Queries;
using PlataformaServicos.DTOs.Prestadores;
using PlataformaServicos.Models;

namespace PlataformaServicos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestadoresController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PrestadoresController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<Prestador>>> Listar()
            => Ok(await _mediator.Send(new ListarPrestadoresQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<Prestador>> BuscarPorId(int id)
        {
            var prestador = await _mediator.Send(new BuscarPrestadorPorIdQuery(id));
            return prestador == null ? NotFound() : Ok(prestador);
        }

        [HttpPost]
        public async Task<ActionResult<Prestador>> Criar(CriarPrestadorDto dto)
        {
            var novo = await _mediator.Send(new CriarPrestadorCommand(dto.Nome, dto.Email, dto.Especialidade, dto.NotaMedia));
            return CreatedAtAction(nameof(BuscarPorId), new { id = novo.Id }, novo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, AtualizarPrestadorDto dto)
        {
            var atualizado = await _mediator.Send(new AtualizarPrestadorCommand(id, dto.Nome, dto.Email, dto.Especialidade, dto.NotaMedia));
            return atualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var deletado = await _mediator.Send(new DeletarPrestadorCommand(id));
            return deletado ? NoContent() : NotFound();
        }
    }
}
