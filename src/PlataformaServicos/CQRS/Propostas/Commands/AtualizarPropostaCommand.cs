using MediatR;

namespace PlataformaServicos.CQRS.Propostas.Commands
{
    public record AtualizarPropostaCommand(int Id, string Titulo, string Descricao, decimal Valor, string Status) : IRequest<bool>;
}
