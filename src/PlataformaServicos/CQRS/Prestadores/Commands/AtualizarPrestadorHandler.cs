using MediatR;
using PlataformaServicos.Data;

namespace PlataformaServicos.CQRS.Prestadores.Commands
{
    public class AtualizarPrestadorHandler : IRequestHandler<AtualizarPrestadorCommand, bool>
    {
        private readonly AppDbContext _context;
        public AtualizarPrestadorHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(AtualizarPrestadorCommand request, CancellationToken cancellationToken)
        {
            var prestador = await _context.Prestadores.FindAsync(request.Id);
            if (prestador == null) return false;
            prestador.Nome = request.Nome;
            prestador.Email = request.Email;
            prestador.Especialidade = request.Especialidade;
            prestador.NotaMedia = request.NotaMedia;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
