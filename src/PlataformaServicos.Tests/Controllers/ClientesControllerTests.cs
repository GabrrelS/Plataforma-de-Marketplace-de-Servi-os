using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlataformaServicos.Controllers;
using PlataformaServicos.CQRS.Clientes.Commands;
using PlataformaServicos.CQRS.Clientes.Queries;
using PlataformaServicos.DTOs.Clientes;
using PlataformaServicos.Models;

namespace PlataformaServicos.Tests.Controllers;

public class ClientesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ClientesController _controller;

    public ClientesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ClientesController(_mediatorMock.Object);
    }

    // ────────────────────────────────────────────────────────────
    //  GET /api/clientes
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Listar_DeveRetornar200ComListaDeClientes()
    {
        // Arrange
        var clientes = new List<Cliente>
        {
            new() { Id = 1, Nome = "Ana", Email = "ana@email.com", Telefone = "1111" },
            new() { Id = 2, Nome = "Beto", Email = "beto@email.com", Telefone = "2222" }
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ListarClientesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientes);

        // Act
        var result = await _controller.Listar();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var retorno = okResult.Value.Should().BeAssignableTo<List<Cliente>>().Subject;
        retorno.Should().HaveCount(2);
    }

    // ────────────────────────────────────────────────────────────
    //  GET /api/clientes/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task BuscarPorId_QuandoExiste_DeveRetornar200()
    {
        // Arrange
        var cliente = new Cliente { Id = 1, Nome = "Carlos", Email = "c@c.com", Telefone = "3333" };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BuscarClientePorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _controller.BuscarPorId(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(cliente);
    }

    [Fact]
    public async Task BuscarPorId_QuandoNaoExiste_DeveRetornar404()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BuscarClientePorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var result = await _controller.BuscarPorId(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    // ────────────────────────────────────────────────────────────
    //  POST /api/clientes
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Criar_DeveRetornar201ComClienteCriado()
    {
        // Arrange
        var dto = new CriarClienteDto { Nome = "Daniela", Email = "d@d.com", Telefone = "4444" };
        var clienteCriado = new Cliente { Id = 5, Nome = dto.Nome, Email = dto.Email, Telefone = dto.Telefone };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CriarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clienteCriado);

        // Act
        var result = await _controller.Criar(dto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(clienteCriado);
    }

    [Fact]
    public async Task Criar_DeveEnviarCommandComDadosDoDto()
    {
        // Arrange
        var dto = new CriarClienteDto { Nome = "Eduardo", Email = "e@e.com", Telefone = "5555" };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CriarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Cliente { Id = 1, Nome = dto.Nome, Email = dto.Email, Telefone = dto.Telefone });

        // Act
        await _controller.Criar(dto);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(
                It.Is<CriarClienteCommand>(c =>
                    c.Nome == dto.Nome &&
                    c.Email == dto.Email &&
                    c.Telefone == dto.Telefone),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ────────────────────────────────────────────────────────────
    //  PUT /api/clientes/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Atualizar_QuandoSucesso_DeveRetornar204()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var dto = new AtualizarClienteDto { Nome = "Novo", Email = "novo@email.com", Telefone = "9999" };

        // Act
        var result = await _controller.Atualizar(1, dto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Atualizar_QuandoNaoEncontrado_DeveRetornar404()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var dto = new AtualizarClienteDto { Nome = "x", Email = "x@x.com", Telefone = "0" };

        // Act
        var result = await _controller.Atualizar(999, dto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    // ────────────────────────────────────────────────────────────
    //  DELETE /api/clientes/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Deletar_QuandoSucesso_DeveRetornar204()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeletarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Deletar(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Deletar_QuandoNaoEncontrado_DeveRetornar404()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeletarClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Deletar(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
