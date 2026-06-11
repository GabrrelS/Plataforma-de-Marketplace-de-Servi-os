using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlataformaServicos.Controllers;
using PlataformaServicos.CQRS.Prestadores.Commands;
using PlataformaServicos.CQRS.Prestadores.Queries;
using PlataformaServicos.DTOs.Prestadores;
using PlataformaServicos.Models;

namespace PlataformaServicos.Tests.Controllers;

public class PrestadoresControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PrestadoresController _controller;

    public PrestadoresControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PrestadoresController(_mediatorMock.Object);
    }

    // ────────────────────────────────────────────────────────────
    //  GET /api/prestadores
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Listar_DeveRetornar200ComListaDePrestadores()
    {
        // Arrange
        var prestadores = new List<Prestador>
        {
            new() { Id = 1, Nome = "Carlos", Email = "carlos@email.com", Especialidade = "Encanamento", NotaMedia = 4.5m },
            new() { Id = 2, Nome = "Fernanda", Email = "fernanda@email.com", Especialidade = "Elétrica", NotaMedia = 4.8m }
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ListarPrestadoresQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(prestadores);

        // Act
        var result = await _controller.Listar();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var retorno = okResult.Value.Should().BeAssignableTo<List<Prestador>>().Subject;
        retorno.Should().HaveCount(2);
    }

    // ────────────────────────────────────────────────────────────
    //  GET /api/prestadores/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task BuscarPorId_QuandoExiste_DeveRetornar200()
    {
        // Arrange
        var prestador = new Prestador { Id = 1, Nome = "Carlos", Email = "carlos@email.com", Especialidade = "Encanamento", NotaMedia = 4.5m };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BuscarPrestadorPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(prestador);

        // Act
        var result = await _controller.BuscarPorId(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(prestador);
    }

    [Fact]
    public async Task BuscarPorId_QuandoNaoExiste_DeveRetornar404()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BuscarPrestadorPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Prestador?)null);

        // Act
        var result = await _controller.BuscarPorId(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    // ────────────────────────────────────────────────────────────
    //  POST /api/prestadores
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Criar_DeveRetornar201ComPrestadorCriado()
    {
        // Arrange
        var dto = new CriarPrestadorDto { Nome = "Lucas", Email = "lucas@email.com", Especialidade = "Pintura", NotaMedia = 4.2m };
        var prestadorCriado = new Prestador { Id = 3, Nome = dto.Nome, Email = dto.Email, Especialidade = dto.Especialidade, NotaMedia = dto.NotaMedia };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CriarPrestadorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(prestadorCriado);

        // Act
        var result = await _controller.Criar(dto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(prestadorCriado);
    }

    [Fact]
    public async Task Criar_DeveEnviarCommandComDadosDoDto()
    {
        // Arrange
        var dto = new CriarPrestadorDto { Nome = "Mariana", Email = "mariana@email.com", Especialidade = "Jardinagem", NotaMedia = 3.9m };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CriarPrestadorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Prestador { Id = 1, Nome = dto.Nome, Email = dto.Email, Especialidade = dto.Especialidade, NotaMedia = dto.NotaMedia });

        // Act
        await _controller.Criar(dto);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(
                It.Is<CriarPrestadorCommand>(c =>
                    c.Nome == dto.Nome &&
                    c.Email == dto.Email &&
                    c.Especialidade == dto.Especialidade &&
                    c.NotaMedia == dto.NotaMedia),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ────────────────────────────────────────────────────────────
    //  PUT /api/prestadores/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Atualizar_QuandoSucesso_DeveRetornar204()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarPrestadorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var dto = new AtualizarPrestadorDto { Nome = "Novo Nome", Email = "novo@email.com", Especialidade = "Hidráulica", NotaMedia = 5.0m };

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
            .Setup(m => m.Send(It.IsAny<AtualizarPrestadorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var dto = new AtualizarPrestadorDto { Nome = "x", Email = "x@x.com", Especialidade = "x", NotaMedia = 0 };

        // Act
        var result = await _controller.Atualizar(999, dto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    // ────────────────────────────────────────────────────────────
    //  DELETE /api/prestadores/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Deletar_QuandoSucesso_DeveRetornar204()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeletarPrestadorCommand>(), It.IsAny<CancellationToken>()))
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
            .Setup(m => m.Send(It.IsAny<DeletarPrestadorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Deletar(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
