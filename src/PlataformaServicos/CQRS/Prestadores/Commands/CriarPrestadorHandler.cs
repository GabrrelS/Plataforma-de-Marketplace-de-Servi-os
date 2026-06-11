using MediatR;
using PlataformaServicos.Data;
using PlataformaServicos.Metrics;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Prestadores.Commands
{
    public class CriarPrestadorHandler : IRequestHandler<CriarPrestadorCommand, Prestador>
    {
        private readonly AppDbContext _context;
        public CriarPrestadorHandler(AppDbContext context) => _context = context;

        public async Task<Prestador> Handle(CriarPrestadorCommand request, CancellationToken cancellationToken)
        {
            var prestador = new Prestador
            {
                Nome = request.Nome,
                Email = request.Email,
                Especialidade = request.Especialidade,
                NotaMedia = request.NotaMedia
            };
            _context.Prestadores.Add(prestador);
            await _context.SaveChangesAsync(cancellationToken);
            MarketplaceMetrics.PrestadoresCriados.Inc();
            return prestador;
        }
    }
}
