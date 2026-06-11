using MediatR;

namespace PlataformaServicos.CQRS.Clientes.Commands
{
    public record AtualizarClienteCommand(int Id, string Nome, string Email, string Telefone) : IRequest<bool>;
}
