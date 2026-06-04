using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;
using PlataformaServicos.Metrics;

namespace PlataformaServicos.Services
{
    public class PropostaService
    {
        private readonly AppDbContext _context;

        public PropostaService(AppDbContext context)
        {
            _context = context;
        }

        // LISTAGEM COM PAGINAÇÃO
        public async Task<List<Proposta>> ListarAsync(
            int pagina = 1,
            int tamanhoPagina = 10)
        {
            return await _context.Propostas
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
        }

        public async Task<Proposta?> BuscarPorIdAsync(int id)
        {
            return await _context.Propostas.FindAsync(id);
        }

        public async Task<Proposta> CriarAsync(Proposta proposta)
        {
            // REGRA DE NEGÓCIO
            if (proposta.Valor <= 0)
            {
                throw new ArgumentException(
                    "O valor da proposta deve ser maior que zero.");
            }

            try
            {
                _context.Propostas.Add(proposta);

                await _context.SaveChangesAsync();

                // MÉTRICA PROMETHEUS
                MarketplaceMetrics.PropostasCriadas.Inc();

                return proposta;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao criar proposta: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarAsync(
            int id,
            Proposta propostaAtualizada)
        {
            var proposta = await _context.Propostas.FindAsync(id);

            if (proposta == null)
                return false;

            if (propostaAtualizada.Valor <= 0)
            {
                throw new ArgumentException(
                    "O valor da proposta deve ser maior que zero.");
            }

            try
            {
                proposta.Titulo = propostaAtualizada.Titulo;
                proposta.Descricao = propostaAtualizada.Descricao;
                proposta.Valor = propostaAtualizada.Valor;
                proposta.Status = propostaAtualizada.Status;

                // ClienteId e PrestadorId NÃO mudam após criação

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao atualizar proposta: {ex.Message}");
            }
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var proposta = await _context.Propostas.FindAsync(id);

            if (proposta == null)
                return false;

            try
            {
                _context.Propostas.Remove(proposta);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao excluir proposta: {ex.Message}");
            }
        }
    }
}