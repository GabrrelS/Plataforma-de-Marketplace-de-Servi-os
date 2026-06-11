using MediatR;
using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Propostas.Queries
{
    public class ListarPropostasHandler : IRequestHandler<ListarPropostasQuery, List<Proposta>>
    {
        private readonly AppDbContext _context;
        public ListarPropostasHandler(AppDbContext context) => _context = context;

        public async Task<List<Proposta>> Handle(ListarPropostasQuery request, CancellationToken cancellationToken)
            => await _context.Propostas.ToListAsync(cancellationToken);
    }
}
