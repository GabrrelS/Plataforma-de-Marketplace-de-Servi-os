using MediatR;
using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Clientes.Queries
{
    public class ListarClientesHandler : IRequestHandler<ListarClientesQuery, List<Cliente>>
    {
        private readonly AppDbContext _context;
        public ListarClientesHandler(AppDbContext context) => _context = context;

        public async Task<List<Cliente>> Handle(ListarClientesQuery request, CancellationToken cancellationToken)
            => await _context.Clientes.ToListAsync(cancellationToken);
    }
}
