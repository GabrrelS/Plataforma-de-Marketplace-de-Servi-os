using MediatR;
using PlataformaServicos.Data;

namespace PlataformaServicos.CQRS.Prestadores.Commands
{
    public class DeletarPrestadorHandler : IRequestHandler<DeletarPrestadorCommand, bool>
    {
        private readonly AppDbContext _context;
        public DeletarPrestadorHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(DeletarPrestadorCommand request, CancellationToken cancellationToken)
        {
            var prestador = await _context.Prestadores.FindAsync(new object[] { request.Id }, cancellationToken);
            if (prestador == null) return false;
            _context.Prestadores.Remove(prestador);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
