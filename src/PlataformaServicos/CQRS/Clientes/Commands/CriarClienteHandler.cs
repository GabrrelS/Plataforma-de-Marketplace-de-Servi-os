using MediatR;
using PlataformaServicos.Data;
using PlataformaServicos.Metrics;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Clientes.Commands
{
    public class CriarClienteHandler : IRequestHandler<CriarClienteCommand, Cliente>
    {
        private readonly AppDbContext _context;
        public CriarClienteHandler(AppDbContext context) => _context = context;

        public async Task<Cliente> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = new Cliente
            {
                Nome = request.Nome,
                Email = request.Email,
                Telefone = request.Telefone
            };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync(cancellationToken);
            MarketplaceMetrics.ClientesCriados.Inc();
            return cliente;
        }
    }
}
