using MediatR;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Features.Clientes.Queries;

// A Query define a intenção de leitura (Listar todos)
public record ListarClientesQuery() : IRequest<List<Cliente>>;

// O Handler recebe a Query e busca os dados através do Service original
public class ListarClientesQueryHandler : IRequestHandler<ListarClientesQuery, List<Cliente>>
{
    private readonly ClienteService _service;

    public ListarClientesQueryHandler(ClienteService service)
    {
        _service = service;
    }

    public async Task<List<Cliente>> Handle(ListarClientesQuery request, CancellationToken cancellationToken)
    {
        return await _service.ListarAsync();
    }
}