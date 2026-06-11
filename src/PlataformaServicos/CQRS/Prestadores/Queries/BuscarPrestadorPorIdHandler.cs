using MediatR;
using PlataformaServicos.Data;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Prestadores.Queries
{
    public class BuscarPrestadorPorIdHandler : IRequestHandler<BuscarPrestadorPorIdQuery, Prestador?>
    {
        private readonly AppDbContext _context;
        public BuscarPrestadorPorIdHandler(AppDbContext context) => _context = context;

        public async Task<Prestador?> Handle(BuscarPrestadorPorIdQuery request, CancellationToken cancellationToken)
            => await _context.Prestadores.FindAsync(request.Id);
    }
}
