using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Propostas.Queries
{
    public record BuscarPropostaPorIdQuery(int Id) : IRequest<Proposta?>;
}
