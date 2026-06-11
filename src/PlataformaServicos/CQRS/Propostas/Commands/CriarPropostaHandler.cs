using MediatR;
using PlataformaServicos.Data;
using PlataformaServicos.Metrics;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Propostas.Commands
{
    public class CriarPropostaHandler : IRequestHandler<CriarPropostaCommand, Proposta>
    {
        private readonly AppDbContext _context;
        public CriarPropostaHandler(AppDbContext context) => _context = context;

        public async Task<Proposta> Handle(CriarPropostaCommand request, CancellationToken cancellationToken)
        {
            var proposta = new Proposta
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                Valor = request.Valor,
                Status = "Pendente",
                ClienteId = request.ClienteId,
                PrestadorId = request.PrestadorId
            };
            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync(cancellationToken);
            MarketplaceMetrics.PropostasCriadas.Inc();
            return proposta;
        }
    }
}
