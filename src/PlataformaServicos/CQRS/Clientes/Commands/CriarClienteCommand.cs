using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Clientes.Commands
{
    public record CriarClienteCommand(string Nome, string Email, string Telefone) : IRequest<Cliente>;
}
