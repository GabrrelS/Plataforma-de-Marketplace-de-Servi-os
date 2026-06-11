using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Prestadores.Queries
{
    public record ListarPrestadoresQuery() : IRequest<List<Prestador>>;
}
