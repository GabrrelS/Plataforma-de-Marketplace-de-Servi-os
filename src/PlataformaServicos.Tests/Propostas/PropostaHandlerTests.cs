using FluentAssertions;
using PlataformaServicos.CQRS.Propostas.Commands;
using PlataformaServicos.CQRS.Propostas.Queries;
using PlataformaServicos.Models;
using PlataformaServicos.Tests.Helpers;

namespace PlataformaServicos.Tests.Propostas;

public class PropostaHandlerTests
{
    // ────────────────────────────────────────────────────────────
    //  CriarPropostaHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task CriarProposta_DeveRetornarPropostaComStatusPendente()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new CriarPropostaHandler(context);
        var command = new CriarPropostaCommand(
            Titulo: "Instalação elétrica",
            Descricao: "Trocar toda a fiação da casa",
            Valor: 1500m,
            ClienteId: 1,
            PrestadorId: 2
        );

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Status.Should().Be("Pendente");
    }

    [Fact]
    public async Task CriarProposta_DeveRetornarPropostaComDadosCorretos()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new CriarPropostaHandler(context);
        var command = new CriarPropostaCommand("Pintura", "Pintar sala e quartos", 800m, 10, 20);

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Titulo.Should().Be("Pintura");
        resultado.Descricao.Should().Be("Pintar sala e quartos");
        resultado.Valor.Should().Be(800m);
        resultado.ClienteId.Should().Be(10);
        resultado.PrestadorId.Should().Be(20);
    }

    [Fact]
    public async Task CriarProposta_DevePersistirNoBancoDeDados()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new CriarPropostaHandler(context);
        var command = new CriarPropostaCommand("Limpeza", "Faxina completa", 300m, 1, 1);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        context.Propostas.Should().HaveCount(1);
    }

    // ────────────────────────────────────────────────────────────
    //  AtualizarPropostaHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task AtualizarProposta_QuandoExiste_DeveAtualizarCamposERetornarTrue()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var proposta = new Proposta
        {
            Titulo = "Serviço Antigo",
            Descricao = "Descrição antiga",
            Valor = 100m,
            Status = "Pendente",
            ClienteId = 1,
            PrestadorId = 1
        };
        context.Propostas.Add(proposta);
        await context.SaveChangesAsync();

        var handler = new AtualizarPropostaHandler(context);
        var command = new AtualizarPropostaCommand(proposta.Id, "Serviço Novo", "Nova descrição", 250m, "Aprovado");

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        var atualizada = context.Propostas.Find(proposta.Id)!;
        atualizada.Titulo.Should().Be("Serviço Novo");
        atualizada.Status.Should().Be("Aprovado");
        atualizada.Valor.Should().Be(250m);
    }

    [Fact]
    public async Task AtualizarProposta_QuandoNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new AtualizarPropostaHandler(context);
        var command = new AtualizarPropostaCommand(999, "Nada", "Nada", 0m, "Pendente");

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
    }

    // ────────────────────────────────────────────────────────────
    //  DeletarPropostaHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeletarProposta_QuandoExiste_DeveRemoverERetornarTrue()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var proposta = new Proposta
        {
            Titulo = "Para Deletar",
            Descricao = "Desc",
            Valor = 50m,
            Status = "Pendente",
            ClienteId = 1,
            PrestadorId = 1
        };
        context.Propostas.Add(proposta);
        await context.SaveChangesAsync();

        var handler = new DeletarPropostaHandler(context);

        // Act
        var resultado = await handler.Handle(new DeletarPropostaCommand(proposta.Id), CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        context.Propostas.Should().BeEmpty();
    }

    [Fact]
    public async Task DeletarProposta_QuandoNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new DeletarPropostaHandler(context);

        // Act
        var resultado = await handler.Handle(new DeletarPropostaCommand(404), CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
    }

    // ────────────────────────────────────────────────────────────
    //  BuscarPropostaPorIdHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task BuscarPropostaPorId_QuandoExiste_DeveRetornarProposta()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var proposta = new Proposta
        {
            Titulo = "Reforma",
            Descricao = "Reforma do banheiro",
            Valor = 2000m,
            Status = "Pendente",
            ClienteId = 5,
            PrestadorId = 3
        };
        context.Propostas.Add(proposta);
        await context.SaveChangesAsync();

        var handler = new BuscarPropostaPorIdHandler(context);

        // Act
        var resultado = await handler.Handle(new BuscarPropostaPorIdQuery(proposta.Id), CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Titulo.Should().Be("Reforma");
        resultado.Valor.Should().Be(2000m);
    }

    [Fact]
    public async Task BuscarPropostaPorId_QuandoNaoExiste_DeveRetornarNull()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var handler = new BuscarPropostaPorIdHandler(context);

        // Act
        var resultado = await handler.Handle(new BuscarPropostaPorIdQuery(999), CancellationToken.None);

        // Assert
        resultado.Should().BeNull();
    }

    // ────────────────────────────────────────────────────────────
    //  ListarPropostasHandler
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task ListarPropostas_ComRegistros_DeveRetornarTodas()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        context.Propostas.AddRange(
            new Proposta { Titulo = "P1", Descricao = "D1", Valor = 100m, Status = "Pendente", ClienteId = 1, PrestadorId = 1 },
            new Proposta { Titulo = "P2", Descricao = "D2", Valor = 200m, Status = "Aprovado", ClienteId = 2, PrestadorId = 2 }
        );
        await context.SaveChangesAsync();

        var handler = new ListarPropostasHandler(context);

        // Act
        var resultado = await handler.Handle(new ListarPropostasQuery(), CancellationToken.None);

        // Assert
        resultado.Should().HaveCount(2);
    }

    // ────────────────────────────────────────────────────────────
    //  Regras de negócio
    // ────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("Pendente")]
    [InlineData("Aprovado")]
    [InlineData("Recusado")]
    [InlineData("Concluído")]
    public async Task AtualizarProposta_VariosStatus_DevePersistirCorretamente(string novoStatus)
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var proposta = new Proposta
        {
            Titulo = "Teste",
            Descricao = "Desc",
            Valor = 100m,
            Status = "Pendente",
            ClienteId = 1,
            PrestadorId = 1
        };
        context.Propostas.Add(proposta);
        await context.SaveChangesAsync();

        var handler = new AtualizarPropostaHandler(context);
        var command = new AtualizarPropostaCommand(proposta.Id, "Teste", "Desc", 100m, novoStatus);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        context.Propostas.Find(proposta.Id)!.Status.Should().Be(novoStatus);
    }
}
