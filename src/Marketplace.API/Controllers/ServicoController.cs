using Marketplace.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ServicoController : ControllerBase
    {

        private static readonly List<Servico> _servicos = new List<Servico>
        {
            new Servico { Id = Guid.NewGuid(), Nome = "Consultoria Tech", Descricao = "Ajuda com código", Preco = 150.00M, Ativo = true },
            new Servico { Id = Guid.NewGuid(), Nome = "Design de Logo", Descricao = "Criação de marcas", Preco = 200.00M, Ativo = true }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Servico>> ListarTodos()
        {
            return Ok(_servicos);
        }

        [HttpPost]
        public ActionResult<Servico> Criar(Servico novoServico)
        {
            novoServico.Id = Guid.NewGuid();
            _servicos.Add(novoServico);
            return CreatedAtAction(nameof(ListarTodos), new { id = novoServico.Id }, novoServico);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(Guid id)
        {
            var servico = _servicos.FirstOrDefault(s => s.Id == id);
            if (servico == null) return NotFound("Serviço não encontrado!");

            _servicos.Remove(servico);
            return NoContent();
        }
    }
}