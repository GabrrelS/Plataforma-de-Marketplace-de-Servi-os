using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Prestadores.Commands
{
    public record CriarPrestadorCommand(string Nome, string Email, string Especialidade, decimal NotaMedia) : IRequest<Prestador>;
}
