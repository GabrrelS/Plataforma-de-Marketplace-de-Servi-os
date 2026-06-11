using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Clientes.Queries
{
    public record BuscarClientePorIdQuery(int Id) : IRequest<Cliente?>;
}
