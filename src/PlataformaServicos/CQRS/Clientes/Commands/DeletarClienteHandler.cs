using MediatR;
using PlataformaServicos.Data;

namespace PlataformaServicos.CQRS.Clientes.Commands
{
    public class DeletarClienteHandler : IRequestHandler<DeletarClienteCommand, bool>
    {
        private readonly AppDbContext _context;
        public DeletarClienteHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(DeletarClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _context.Clientes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (cliente == null) return false;
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
