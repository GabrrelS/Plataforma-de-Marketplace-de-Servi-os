using MediatR;
using PlataformaServicos.Data;

namespace PlataformaServicos.CQRS.Propostas.Commands
{
    public class AtualizarPropostaHandler : IRequestHandler<AtualizarPropostaCommand, bool>
    {
        private readonly AppDbContext _context;
        public AtualizarPropostaHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(AtualizarPropostaCommand request, CancellationToken cancellationToken)
        {
            var proposta = await _context.Propostas.FindAsync(new object[] { request.Id }, cancellationToken);
            if (proposta == null) return false;
            proposta.Titulo = request.Titulo;
            proposta.Descricao = request.Descricao;
            proposta.Valor = request.Valor;
            proposta.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
