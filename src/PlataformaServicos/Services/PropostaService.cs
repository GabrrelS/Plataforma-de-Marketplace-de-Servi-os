using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;

namespace PlataformaServicos.Services
{
    public class PropostaService
    {
        private readonly AppDbContext _context;

        public PropostaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proposta>> ListarAsync()
        {
            return await _context.Propostas.ToListAsync();
        }

        public async Task<Proposta?> BuscarPorIdAsync(int id)
        {
            return await _context.Propostas.FindAsync(id);
        }

        public async Task<Proposta> CriarAsync(Proposta proposta)
        {
            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();
            return proposta;
        }

        public async Task<bool> AtualizarAsync(int id, Proposta propostaAtualizada)
        {
            var proposta = await _context.Propostas.FindAsync(id);

            if (proposta == null)
                return false;

            proposta.Titulo = propostaAtualizada.Titulo;
            proposta.Descricao = propostaAtualizada.Descricao;
            proposta.Valor = propostaAtualizada.Valor;
            proposta.Status = propostaAtualizada.Status;
            proposta.ClienteId = propostaAtualizada.ClienteId;
            proposta.PrestadorId = propostaAtualizada.PrestadorId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var proposta = await _context.Propostas.FindAsync(id);

            if (proposta == null)
                return false;

            _context.Propostas.Remove(proposta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}