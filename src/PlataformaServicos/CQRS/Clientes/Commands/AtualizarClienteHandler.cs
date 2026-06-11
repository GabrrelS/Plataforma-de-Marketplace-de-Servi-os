using MediatR;
using PlataformaServicos.Data;

namespace PlataformaServicos.CQRS.Clientes.Commands
{
    public class AtualizarClienteHandler : IRequestHandler<AtualizarClienteCommand, bool>
    {
        private readonly AppDbContext _context;
        public AtualizarClienteHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(AtualizarClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _context.Clientes.FindAsync(new object[] { request.Id }, cancellationToken);
            if (cliente == null) return false;
            cliente.Nome = request.Nome;
            cliente.Email = request.Email;
            cliente.Telefone = request.Telefone;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
