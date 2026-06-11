using FluentAssertions;
using PlataformaServicos.CQRS.Clientes.Commands;
using PlataformaServicos.CQRS.Clientes.Queries;
using PlataformaServicos.Models;
using PlataformaServicos.Tests.Helpers;

namespace PlataformaServicos.Tests.Clientes;

public class ClienteHandlerTests
{
    // ────────────────────────────────────────────────────────────
    //  CriarClienteHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task CriarCliente_DeveRetornarClienteComDadosCorretos()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new CriarClienteHandler(context);
        var command = new CriarClienteCommand("Maria Silva", "maria@email.com", "83900000001");

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be("Maria Silva");
        resultado.Email.Should().Be("maria@email.com");
        resultado.Telefone.Should().Be("83900000001");
    }

    [Fact]
    public async Task CriarCliente_DevePersistirNoBancoDeDados()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new CriarClienteHandler(context);
        var command = new CriarClienteCommand("João Costa", "joao@email.com", "83900000002");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        context.Clientes.Should().HaveCount(1);
        context.Clientes.First().Email.Should().Be("joao@email.com");
    }

    [Fact]
    public async Task CriarCliente_DeveAtribuirIdAutoGerado()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new CriarClienteHandler(context);
        var command = new CriarClienteCommand("Ana Souza", "ana@email.com", "83900000003");

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Id.Should().BeGreaterThan(0);
    }

    // ────────────────────────────────────────────────────────────
    //  AtualizarClienteHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task AtualizarCliente_QuandoClienteExiste_DeveRetornarTrue()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var cliente = new Cliente { Nome = "Pedro", Email = "pedro@email.com", Telefone = "1111" };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var handler = new AtualizarClienteHandler(context);
        var command = new AtualizarClienteCommand(cliente.Id, "Pedro Atualizado", "novo@email.com", "2222");

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        context.Clientes.Find(cliente.Id)!.Nome.Should().Be("Pedro Atualizado");
        context.Clientes.Find(cliente.Id)!.Email.Should().Be("novo@email.com");
    }

    [Fact]
    public async Task AtualizarCliente_QuandoClienteNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new AtualizarClienteHandler(context);
        var command = new AtualizarClienteCommand(999, "Fantasma", "x@x.com", "0000");

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
    }

    // ────────────────────────────────────────────────────────────
    //  DeletarClienteHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeletarCliente_QuandoClienteExiste_DeveRemoverERetornarTrue()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var cliente = new Cliente { Nome = "Carlos", Email = "carlos@email.com", Telefone = "3333" };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var handler = new DeletarClienteHandler(context);
        var command = new DeletarClienteCommand(cliente.Id);

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        context.Clientes.Should().BeEmpty();
    }

    [Fact]
    public async Task DeletarCliente_QuandoClienteNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new DeletarClienteHandler(context);
        var command = new DeletarClienteCommand(999);

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
    }

    // ────────────────────────────────────────────────────────────
    //  BuscarClientePorIdHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task BuscarClientePorId_QuandoExiste_DeveRetornarCliente()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var cliente = new Cliente { Nome = "Lucia", Email = "lucia@email.com", Telefone = "4444" };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var handler = new BuscarClientePorIdHandler(context);
        var query = new BuscarClientePorIdQuery(cliente.Id);

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be("Lucia");
    }

    [Fact]
    public async Task BuscarClientePorId_QuandoNaoExiste_DeveRetornarNull()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new BuscarClientePorIdHandler(context);
        var query = new BuscarClientePorIdQuery(999);

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().BeNull();
    }

    // ────────────────────────────────────────────────────────────
    //  ListarClientesHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task ListarClientes_ComDoisClientes_DeveRetornarListaCorreta()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        context.Clientes.AddRange(
            new Cliente { Nome = "A", Email = "a@a.com", Telefone = "1" },
            new Cliente { Nome = "B", Email = "b@b.com", Telefone = "2" }
        );
        await context.SaveChangesAsync();

        var handler = new ListarClientesHandler(context);
        var query = new ListarClientesQuery();

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListarClientes_SemClientes_DeveRetornarListaVazia()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new ListarClientesHandler(context);

        // Act
        var resultado = await handler.Handle(new ListarClientesQuery(), CancellationToken.None);

        // Assert
        resultado.Should().BeEmpty();
    }
}
