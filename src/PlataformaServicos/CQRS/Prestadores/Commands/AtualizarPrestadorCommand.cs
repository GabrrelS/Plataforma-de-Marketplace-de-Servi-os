using MediatR;

namespace PlataformaServicos.CQRS.Prestadores.Commands
{
    public record AtualizarPrestadorCommand(int Id, string Nome, string Email, string Especialidade, decimal NotaMedia) : IRequest<bool>;
}
