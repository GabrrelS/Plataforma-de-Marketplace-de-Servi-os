using MediatR;

namespace PlataformaServicos.CQRS.Prestadores.Commands
{
    public record DeletarPrestadorCommand(int Id) : IRequest<bool>;
}
