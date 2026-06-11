using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Clientes.Queries
{
    public record ListarClientesQuery() : IRequest<List<Cliente>>;
}
