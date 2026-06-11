using MediatR;
using PlataformaServicos.Models;

namespace PlataformaServicos.CQRS.Propostas.Commands
{
    public record CriarPropostaCommand(string Titulo, string Descricao, decimal Valor, int ClienteId, int PrestadorId) : IRequest<Proposta>;
}
