using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlataformaServicos.Controllers;
using PlataformaServicos.CQRS.Propostas.Commands;
using PlataformaServicos.CQRS.Propostas.Queries;
using PlataformaServicos.DTOs.Propostas;
using PlataformaServicos.Models;

namespace PlataformaServicos.Tests.Controllers;

public class PropostasControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PropostasController _controller;

    public PropostasControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PropostasController(_mediatorMock.Object);
    }

    // ────────────────────────────────────────────────────────────
    //  GET /api/propostas
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Listar_DeveRetornar200ComListaDePropostas()
    {
        // Arrange
        var propostas = new List<Proposta>
        {
            new() { Id = 1, Titulo = "Reforma Banheiro", Descricao = "Troca de azulejos", Valor = 1500m, Status = "Pendente", ClienteId = 1, PrestadorId = 2 },
            new() { Id = 2, Titulo = "Pintura Sala", Descricao = "Pintura completa", Valor = 800m, Status = "Aceita", ClienteId = 3, PrestadorId = 4 }
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ListarPropostasQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(propostas);

        // Act
        var result = await _controller.Listar();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var retorno = okResult.Value.Should().BeAssignableTo<List<Proposta>>().Subject;
        retorno.Should().HaveCount(2);
    }

    // ────────────────────────────────────────────────────────────
    //  GET /api/propostas/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task BuscarPorId_QuandoExiste_DeveRetornar200()
    {
        // Arrange
        var proposta = new Proposta { Id = 1, Titulo = "Reforma Banheiro", Descricao = "Troca de azulejos", Valor = 1500m, Status = "Pendente", ClienteId = 1, PrestadorId = 2 };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BuscarPropostaPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(proposta);

        // Act
        var result = await _controller.BuscarPorId(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(proposta);
    }

    [Fact]
    public async Task BuscarPorId_QuandoNaoExiste_DeveRetornar404()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BuscarPropostaPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Proposta?)null);

        // Act
        var result = await _controller.BuscarPorId(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    // ────────────────────────────────────────────────────────────
    //  POST /api/propostas
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Criar_DeveRetornar201ComPropostaCriada()
    {
        // Arrange
        var dto = new CriarPropostaDto { Titulo = "Instalação Elétrica", Descricao = "Troca do quadro elétrico", Valor = 2200m, ClienteId = 1, PrestadorId = 3 };
        var propostaCriada = new Proposta { Id = 5, Titulo = dto.Titulo, Descricao = dto.Descricao, Valor = dto.Valor, Status = "Pendente", ClienteId = dto.ClienteId, PrestadorId = dto.PrestadorId };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CriarPropostaCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(propostaCriada);

        // Act
        var result = await _controller.Criar(dto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(propostaCriada);
    }

    [Fact]
    public async Task Criar_DeveEnviarCommandComDadosDoDto()
    {
        // Arrange
        var dto = new CriarPropostaDto { Titulo = "Limpeza Caixa D'Água", Descricao = "Limpeza completa", Valor = 350m, ClienteId = 2, PrestadorId = 5 };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CriarPropostaCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Proposta { Id = 1, Titulo = dto.Titulo, Descricao = dto.Descricao, Valor = dto.Valor, ClienteId = dto.ClienteId, PrestadorId = dto.PrestadorId });

        // Act
        await _controller.Criar(dto);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(
                It.Is<CriarPropostaCommand>(c =>
                    c.Titulo == dto.Titulo &&
                    c.Descricao == dto.Descricao &&
                    c.Valor == dto.Valor &&
                    c.ClienteId == dto.ClienteId &&
                    c.PrestadorId == dto.PrestadorId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ────────────────────────────────────────────────────────────
    //  PUT /api/propostas/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Atualizar_QuandoSucesso_DeveRetornar204()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarPropostaCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var dto = new AtualizarPropostaDto { Titulo = "Novo Título", Descricao = "Nova descrição", Valor = 3000m, Status = "Aceita" };

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
            .Setup(m => m.Send(It.IsAny<AtualizarPropostaCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var dto = new AtualizarPropostaDto { Titulo = "x", Descricao = "x", Valor = 1m, Status = "Pendente" };

        // Act
        var result = await _controller.Atualizar(999, dto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    // ────────────────────────────────────────────────────────────
    //  DELETE /api/propostas/{id}
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Deletar_QuandoSucesso_DeveRetornar204()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeletarPropostaCommand>(), It.IsAny<CancellationToken>()))
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
            .Setup(m => m.Send(It.IsAny<DeletarPropostaCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Deletar(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
