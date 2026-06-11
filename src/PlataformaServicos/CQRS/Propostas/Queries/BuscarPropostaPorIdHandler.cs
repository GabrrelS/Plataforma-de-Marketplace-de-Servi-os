using MediatR;
using PlataformaServicos.Data;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Propostas.Queries
{
    public class BuscarPropostaPorIdHandler : IRequestHandler<BuscarPropostaPorIdQuery, Proposta?>
    {
        private readonly AppDbContext _context;
        public BuscarPropostaPorIdHandler(AppDbContext context) => _context = context;

        public async Task<Proposta?> Handle(BuscarPropostaPorIdQuery request, CancellationToken cancellationToken)
            => await _context.Propostas.FindAsync(new object[] { request.Id }, cancellationToken);
    }
}
