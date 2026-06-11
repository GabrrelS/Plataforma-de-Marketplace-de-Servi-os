using MediatR;

namespace PlataformaServicos.CQRS.Clientes.Commands
{
    public record DeletarClienteCommand(int Id) : IRequest<bool>;
}
