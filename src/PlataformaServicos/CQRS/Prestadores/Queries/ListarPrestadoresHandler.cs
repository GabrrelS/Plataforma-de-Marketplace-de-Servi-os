using MediatR;
using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Prestadores.Queries
{
    public class ListarPrestadoresHandler : IRequestHandler<ListarPrestadoresQuery, List<Prestador>>
    {
        private readonly AppDbContext _context;
        public ListarPrestadoresHandler(AppDbContext context) => _context = context;

        public async Task<List<Prestador>> Handle(ListarPrestadoresQuery request, CancellationToken cancellationToken)
            => await _context.Prestadores.ToListAsync(cancellationToken);
    }
}
