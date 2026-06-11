using MediatR;
using PlataformaServicos.Data;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Clientes.Queries
{
    public class BuscarClientePorIdHandler : IRequestHandler<BuscarClientePorIdQuery, Cliente?>
    {
        private readonly AppDbContext _context;
        public BuscarClientePorIdHandler(AppDbContext context) => _context = context;

        public async Task<Cliente?> Handle(BuscarClientePorIdQuery request, CancellationToken cancellationToken)
            => await _context.Clientes.FindAsync(new object[] { request.Id }, cancellationToken);
    }
}
