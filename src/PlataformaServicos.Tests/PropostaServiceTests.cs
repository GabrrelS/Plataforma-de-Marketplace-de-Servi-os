using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Tests;

public class PropostaServiceTests
{
    private AppDbContext CriarContexto(string nomeDb)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: nomeDb)
            .Options;

        return new AppDbContext(options);
    }

    // =====================================================================
    // LISTAR
    // =====================================================================

    [Fact(DisplayName = "Listar retorna lista vazia quando não há propostas")]
    public async Task ListarAsync_SemPropostas_RetornaListaVazia()
    {
        using var ctx = CriarContexto("prop_listar_vazio");
        var service = new PropostaService(ctx);

        var resultado = await service.ListarAsync();

        resultado.Should().BeEmpty();
    }

    [Fact(DisplayName = "Listar retorna todas as propostas cadastradas")]
    public async Task ListarAsync_ComPropostas_RetornaTodas()
    {
        using var ctx = CriarContexto("prop_listar_todas");
        ctx.Propostas.AddRange(
            new Proposta { Titulo = "Site",  Descricao = "Criar site",   Valor = 1000, ClienteId = 1, PrestadorId = 1 },
            new Proposta { Titulo = "App",   Descricao = "Criar app",    Valor = 2000, ClienteId = 2, PrestadorId = 2 },
            new Proposta { Titulo = "Logo",  Descricao = "Criar logo",   Valor = 500,  ClienteId = 1, PrestadorId = 3 }
        );
        await ctx.SaveChangesAsync();

        var service = new PropostaService(ctx);
        var resultado = await service.ListarAsync();

        resultado.Should().HaveCount(3);
        resultado.Should().Contain(p => p.Titulo == "Site");
        resultado.Should().Contain(p => p.Titulo == "App");
    }

    // =====================================================================
    // BUSCAR POR ID
    // =====================================================================

    [Fact(DisplayName = "BuscarPorId retorna proposta quando existe")]
    public async Task BuscarPorIdAsync_PropostaExistente_RetornaProposta()
    {
        using var ctx = CriarContexto("prop_buscar_existente");
        var proposta = new Proposta
        {
            Titulo      = "API REST",
            Descricao   = "Desenvolver API",
            Valor       = 3000,
            Status      = "Pendente",
            ClienteId   = 1,
            PrestadorId = 2
        };
        ctx.Propostas.Add(proposta);
        await ctx.SaveChangesAsync();

        var service = new PropostaService(ctx);
        var resultado = await service.BuscarPorIdAsync(proposta.Id);

        resultado.Should().NotBeNull();
        resultado!.Titulo.Should().Be("API REST");
        resultado.Valor.Should().Be(3000);
        resultado.Status.Should().Be("Pendente");
    }

    [Fact(DisplayName = "BuscarPorId retorna null quando proposta não existe")]
    public async Task BuscarPorIdAsync_PropostaInexistente_RetornaNull()
    {
        using var ctx = CriarContexto("prop_buscar_inexistente");
        var service = new PropostaService(ctx);

        var resultado = await service.BuscarPorIdAsync(999);

        resultado.Should().BeNull();
    }

    // =====================================================================
    // CRIAR
    // =====================================================================

    [Fact(DisplayName = "Criar persiste proposta no banco com status Pendente por padrão")]
    public async Task CriarAsync_DadosValidos_PersisteProposta()
    {
        using var ctx = CriarContexto("prop_criar_valido");
        var service = new PropostaService(ctx);

        var resultado = await service.CriarAsync(new Proposta
        {
            Titulo      = "Dashboard",
            Descricao   = "Criar dashboard analytics",
            Valor       = 1500,
            ClienteId   = 1,
            PrestadorId = 1
        });

        resultado.Id.Should().BeGreaterThan(0);
        resultado.Titulo.Should().Be("Dashboard");
        resultado.Status.Should().Be("Pendente");
        ctx.Propostas.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Criar proposta com valor zero deve persistir normalmente")]
    public async Task CriarAsync_ValorZero_PersisteProposta()
    {
        using var ctx = CriarContexto("prop_criar_zero");
        var service = new PropostaService(ctx);

        var resultado = await service.CriarAsync(new Proposta
        {
            Titulo      = "Consultoria gratuita",
            Descricao   = "Sessão de consultoria",
            Valor       = 0,
            ClienteId   = 1,
            PrestadorId = 1
        });

        resultado.Valor.Should().Be(0);
        resultado.Id.Should().BeGreaterThan(0);
    }

    // =====================================================================
    // ATUALIZAR
    // =====================================================================

    [Fact(DisplayName = "Atualizar muda status da proposta para Aceita")]
    public async Task AtualizarAsync_MudaStatusParaAceita_RetornaTrue()
    {
        using var ctx = CriarContexto("prop_atualizar_status");
        var proposta = new Proposta
        {
            Titulo      = "E-commerce",
            Descricao   = "Loja virtual",
            Valor       = 5000,
            Status      = "Pendente",
            ClienteId   = 1,
            PrestadorId = 1
        };
        ctx.Propostas.Add(proposta);
        await ctx.SaveChangesAsync();

        var service = new PropostaService(ctx);
        var atualizada = new Proposta
        {
            Titulo      = "E-commerce",
            Descricao   = "Loja virtual completa",
            Valor       = 5500,
            Status      = "Aceita",
            ClienteId   = 1,
            PrestadorId = 1
        };

        var resultado = await service.AtualizarAsync(proposta.Id, atualizada);

        resultado.Should().BeTrue();
        var salva = await ctx.Propostas.FindAsync(proposta.Id);
        salva!.Status.Should().Be("Aceita");
        salva.Valor.Should().Be(5500);
    }

    [Fact(DisplayName = "Atualizar retorna false quando proposta não existe")]
    public async Task AtualizarAsync_PropostaInexistente_RetornaFalse()
    {
        using var ctx = CriarContexto("prop_atualizar_inexistente");
        var service = new PropostaService(ctx);

        var resultado = await service.AtualizarAsync(999, new Proposta
        {
            Titulo = "X", Descricao = "Y", Valor = 0, ClienteId = 1, PrestadorId = 1
        });

        resultado.Should().BeFalse();
    }

    // =====================================================================
    // DELETAR
    // =====================================================================

    [Fact(DisplayName = "Deletar remove proposta existente e retorna true")]
    public async Task DeletarAsync_PropostaExistente_RetornaTrue()
    {
        using var ctx = CriarContexto("prop_deletar_existente");
        var proposta = new Proposta
        {
            Titulo = "Blog", Descricao = "Criar blog", Valor = 800, ClienteId = 1, PrestadorId = 1
        };
        ctx.Propostas.Add(proposta);
        await ctx.SaveChangesAsync();

        var service = new PropostaService(ctx);
        var resultado = await service.DeletarAsync(proposta.Id);

        resultado.Should().BeTrue();
        ctx.Propostas.Should().BeEmpty();
    }

    [Fact(DisplayName = "Deletar retorna false quando proposta não existe")]
    public async Task DeletarAsync_PropostaInexistente_RetornaFalse()
    {
        using var ctx = CriarContexto("prop_deletar_inexistente");
        var service = new PropostaService(ctx);

        var resultado = await service.DeletarAsync(999);

        resultado.Should().BeFalse();
    }
}
