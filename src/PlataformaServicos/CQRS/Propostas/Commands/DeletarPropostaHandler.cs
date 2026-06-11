using MediatR;
using PlataformaServicos.Data;

namespace PlataformaServicos.CQRS.Propostas.Commands
{
    public class DeletarPropostaHandler : IRequestHandler<DeletarPropostaCommand, bool>
    {
        private readonly AppDbContext _context;
        public DeletarPropostaHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(DeletarPropostaCommand request, CancellationToken cancellationToken)
        {
            var proposta = await _context.Propostas.FindAsync(request.Id);
            if (proposta == null) return false;
            _context.Propostas.Remove(proposta);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
