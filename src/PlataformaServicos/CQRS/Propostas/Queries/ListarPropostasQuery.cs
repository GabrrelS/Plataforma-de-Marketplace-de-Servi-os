using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Propostas.Queries
{
    public record ListarPropostasQuery() : IRequest<List<Proposta>>;
}
