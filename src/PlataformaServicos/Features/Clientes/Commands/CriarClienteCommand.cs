using MediatR;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Features.Clientes.Commands;

// O Command define quais dados são necessários para a escrita (Criar)
public record CriarClienteCommand(Cliente Cliente) : IRequest<Cliente>;

// O Handler é quem recebe o Command e executa a lógica usando o Service original
public class CriarClienteCommandHandler : IRequestHandler<CriarClienteCommand, Cliente>
{
    private readonly ClienteService _service;

    public CriarClienteCommandHandler(ClienteService service)
    {
        _service = service;
    }

    public async Task<Cliente> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        return await _service.CriarAsync(request.Cliente);
    }
}