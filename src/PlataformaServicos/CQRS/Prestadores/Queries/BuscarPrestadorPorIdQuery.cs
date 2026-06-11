using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Prestadores.Queries
{
    public record BuscarPrestadorPorIdQuery(int Id) : IRequest<Prestador?>;
}
