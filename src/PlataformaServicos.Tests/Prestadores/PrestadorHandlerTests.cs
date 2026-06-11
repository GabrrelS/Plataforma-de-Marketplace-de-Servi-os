using FluentAssertions;
using PlataformaServicos.CQRS.Prestadores.Commands;
using PlataformaServicos.CQRS.Prestadores.Queries;
using PlataformaServicos.Models;
using PlataformaServicos.Tests.Helpers;

namespace PlataformaServicos.Tests.Prestadores;

public class PrestadorHandlerTests
{
    // ────────────────────────────────────────────────────────────
    //  CriarPrestadorHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task CriarPrestador_DeveRetornarPrestadorComDadosCorretos()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new CriarPrestadorHandler(context);
        var command = new CriarPrestadorCommand("Carlos Eletricista", "carlos@email.com", "Elétrica", 4.8m);

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be("Carlos Eletricista");
        resultado.Email.Should().Be("carlos@email.com");
        resultado.Especialidade.Should().Be("Elétrica");
        resultado.NotaMedia.Should().Be(4.8m);
    }

    [Fact]
    public async Task CriarPrestador_DevePersistirNoBancoDeDados()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new CriarPrestadorHandler(context);
        var command = new CriarPrestadorCommand("Roberto Pintor", "roberto@email.com", "Pintura", 4.5m);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        context.Prestadores.Should().HaveCount(1);
        context.Prestadores.First().Especialidade.Should().Be("Pintura");
    }

    // ────────────────────────────────────────────────────────────
    //  AtualizarPrestadorHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task AtualizarPrestador_QuandoExiste_DeveAtualizarERetornarTrue()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var prestador = new Prestador
        {
            Nome = "Jorge",
            Email = "jorge@email.com",
            Especialidade = "Hidráulica",
            NotaMedia = 3.0m
        };
        context.Prestadores.Add(prestador);
        await context.SaveChangesAsync();

        var handler = new AtualizarPrestadorHandler(context);
        var command = new AtualizarPrestadorCommand(prestador.Id, "Jorge Atualizado", "novo@email.com", "Elétrica", 4.9m);

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        var atualizado = context.Prestadores.Find(prestador.Id)!;
        atualizado.Nome.Should().Be("Jorge Atualizado");
        atualizado.Especialidade.Should().Be("Elétrica");
        atualizado.NotaMedia.Should().Be(4.9m);
    }

    [Fact]
    public async Task AtualizarPrestador_QuandoNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new AtualizarPrestadorHandler(context);
        var command = new AtualizarPrestadorCommand(999, "Ninguém", "x@x.com", "Nada", 0m);

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
    }

    // ────────────────────────────────────────────────────────────
    //  DeletarPrestadorHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeletarPrestador_QuandoExiste_DeveRemoverERetornarTrue()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var prestador = new Prestador
        {
            Nome = "Zé Pedreiro",
            Email = "ze@email.com",
            Especialidade = "Pedreiro",
            NotaMedia = 4.0m
        };
        context.Prestadores.Add(prestador);
        await context.SaveChangesAsync();

        var handler = new DeletarPrestadorHandler(context);

        // Act
        var resultado = await handler.Handle(new DeletarPrestadorCommand(prestador.Id), CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        context.Prestadores.Should().BeEmpty();
    }

    [Fact]
    public async Task DeletarPrestador_QuandoNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new DeletarPrestadorHandler(context);

        // Act
        var resultado = await handler.Handle(new DeletarPrestadorCommand(404), CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
    }

    // ────────────────────────────────────────────────────────────
    //  BuscarPrestadorPorIdHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task BuscarPrestadorPorId_QuandoExiste_DeveRetornarPrestador()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var prestador = new Prestador
        {
            Nome = "Fernanda",
            Email = "fernanda@email.com",
            Especialidade = "Limpeza",
            NotaMedia = 5.0m
        };
        context.Prestadores.Add(prestador);
        await context.SaveChangesAsync();

        var handler = new BuscarPrestadorPorIdHandler(context);

        // Act
        var resultado = await handler.Handle(new BuscarPrestadorPorIdQuery(prestador.Id), CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be("Fernanda");
    }

    [Fact]
    public async Task BuscarPrestadorPorId_QuandoNaoExiste_DeveRetornarNull()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new BuscarPrestadorPorIdHandler(context);

        // Act
        var resultado = await handler.Handle(new BuscarPrestadorPorIdQuery(999), CancellationToken.None);

        // Assert
        resultado.Should().BeNull();
    }

    // ────────────────────────────────────────────────────────────
    //  ListarPrestadoresHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task ListarPrestadores_ComRegistros_DeveRetornarTodos()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        context.Prestadores.AddRange(
            new Prestador { Nome = "P1", Email = "p1@p.com", Especialidade = "Elétrica", NotaMedia = 4m },
            new Prestador { Nome = "P2", Email = "p2@p.com", Especialidade = "Pintura", NotaMedia = 4.5m },
            new Prestador { Nome = "P3", Email = "p3@p.com", Especialidade = "Hidráulica", NotaMedia = 3.8m }
        );
        await context.SaveChangesAsync();

        var handler = new ListarPrestadoresHandler(context);

        // Act
        var resultado = await handler.Handle(new ListarPrestadoresQuery(), CancellationToken.None);

        // Assert
        resultado.Should().HaveCount(3);
    }
}
