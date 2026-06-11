using MediatR;

namespace PlataformaServicos.CQRS.Propostas.Commands
{
    public record DeletarPropostaCommand(int Id) : IRequest<bool>;
}
