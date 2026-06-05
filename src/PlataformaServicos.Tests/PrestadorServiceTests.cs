using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Tests;

public class PrestadorServiceTests
{
    // Cria um AppDbContext em memória isolado para cada teste
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

    [Fact(DisplayName = "Listar retorna lista vazia quando não há prestadores")]
    public async Task ListarAsync_SemPrestadores_RetornaListaVazia()
    {
        // Arrange
        using var ctx = CriarContexto("listar_vazio");
        var service = new PrestadorService(ctx);

        // Act
        var resultado = await service.ListarAsync();

        // Assert
        resultado.Should().BeEmpty();
    }

    [Fact(DisplayName = "Listar retorna todos os prestadores cadastrados")]
    public async Task ListarAsync_ComPrestadores_RetornaTodos()
    {
        // Arrange
        using var ctx = CriarContexto("listar_todos");
        ctx.Prestadores.AddRange(
            new Prestador { Nome = "Ana",   Email = "ana@email.com",   Especialidade = "Design" },
            new Prestador { Nome = "Bruno", Email = "bruno@email.com", Especialidade = "Dev"    }
        );
        await ctx.SaveChangesAsync();

        var service = new PrestadorService(ctx);

        // Act
        var resultado = await service.ListarAsync();

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Should().Contain(p => p.Nome == "Ana");
        resultado.Should().Contain(p => p.Nome == "Bruno");
    }

    // =====================================================================
    // BUSCAR POR ID
    // =====================================================================

    [Fact(DisplayName = "BuscarPorId retorna prestador quando existe")]
    public async Task BuscarPorIdAsync_PrestadorExistente_RetornaPrestador()
    {
        // Arrange
        using var ctx = CriarContexto("buscar_existente");
        var prestador = new Prestador { Nome = "Carlos", Email = "carlos@email.com", Especialidade = "Marketing" };
        ctx.Prestadores.Add(prestador);
        await ctx.SaveChangesAsync();

        var service = new PrestadorService(ctx);

        // Act
        var resultado = await service.BuscarPorIdAsync(prestador.Id);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be("Carlos");
        resultado.Especialidade.Should().Be("Marketing");
    }

    [Fact(DisplayName = "BuscarPorId retorna null quando prestador não existe")]
    public async Task BuscarPorIdAsync_PrestadorInexistente_RetornaNull()
    {
        // Arrange
        using var ctx = CriarContexto("buscar_inexistente");
        var service = new PrestadorService(ctx);

        // Act
        var resultado = await service.BuscarPorIdAsync(999);

        // Assert
        resultado.Should().BeNull();
    }

    // =====================================================================
    // CRIAR
    // =====================================================================

    [Fact(DisplayName = "Criar persiste prestador no banco e retorna com Id gerado")]
    public async Task CriarAsync_DadosValidos_PersistePrestador()
    {
        // Arrange
        using var ctx = CriarContexto("criar_valido");
        var service = new PrestadorService(ctx);
        var novoPrestador = new Prestador
        {
            Nome          = "Diana",
            Email         = "diana@email.com",
            Especialidade = "Redação"
        };

        // Act
        var resultado = await service.CriarAsync(novoPrestador);

        // Assert
        resultado.Id.Should().BeGreaterThan(0);
        resultado.Nome.Should().Be("Diana");
        ctx.Prestadores.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Criar dois prestadores gera IDs distintos")]
    public async Task CriarAsync_DoisPrestadores_GeraIdsDiferentes()
    {
        // Arrange
        using var ctx = CriarContexto("criar_dois");
        var service = new PrestadorService(ctx);

        // Act
        var p1 = await service.CriarAsync(new Prestador { Nome = "Eva",   Email = "eva@email.com",   Especialidade = "Dev"    });
        var p2 = await service.CriarAsync(new Prestador { Nome = "Fabio", Email = "fabio@email.com", Especialidade = "Design" });

        // Assert
        p1.Id.Should().NotBe(p2.Id);
    }

    // =====================================================================
    // ATUALIZAR
    // =====================================================================

    [Fact(DisplayName = "Atualizar altera os dados do prestador existente")]
    public async Task AtualizarAsync_PrestadorExistente_RetornaTrue()
    {
        // Arrange
        using var ctx = CriarContexto("atualizar_existente");
        var prestador = new Prestador { Nome = "Gabi", Email = "gabi@email.com", Especialidade = "Design" };
        ctx.Prestadores.Add(prestador);
        await ctx.SaveChangesAsync();

        var service = new PrestadorService(ctx);
        var atualizado = new Prestador { Nome = "Gabriela", Email = "gabriela@novo.com", Especialidade = "UX" };

        // Act
        var resultado = await service.AtualizarAsync(prestador.Id, atualizado);

        // Assert
        resultado.Should().BeTrue();
        var salvo = await ctx.Prestadores.FindAsync(prestador.Id);
        salvo!.Nome.Should().Be("Gabriela");
        salvo.Especialidade.Should().Be("UX");
    }

    [Fact(DisplayName = "Atualizar retorna false quando prestador não existe")]
    public async Task AtualizarAsync_PrestadorInexistente_RetornaFalse()
    {
        // Arrange
        using var ctx = CriarContexto("atualizar_inexistente");
        var service = new PrestadorService(ctx);

        // Act
        var resultado = await service.AtualizarAsync(999, new Prestador { Nome = "X", Email = "x@x.com", Especialidade = "Y" });

        // Assert
        resultado.Should().BeFalse();
    }

    // =====================================================================
    // DELETAR
    // =====================================================================

    [Fact(DisplayName = "Deletar remove prestador existente e retorna true")]
    public async Task DeletarAsync_PrestadorExistente_RetornaTrue()
    {
        // Arrange
        using var ctx = CriarContexto("deletar_existente");
        var prestador = new Prestador { Nome = "Hugo", Email = "hugo@email.com", Especialidade = "Dev" };
        ctx.Prestadores.Add(prestador);
        await ctx.SaveChangesAsync();

        var service = new PrestadorService(ctx);

        // Act
        var resultado = await service.DeletarAsync(prestador.Id);

        // Assert
        resultado.Should().BeTrue();
        ctx.Prestadores.Should().BeEmpty();
    }

    [Fact(DisplayName = "Deletar retorna false quando prestador não existe")]
    public async Task DeletarAsync_PrestadorInexistente_RetornaFalse()
    {
        // Arrange
        using var ctx = CriarContexto("deletar_inexistente");
        var service = new PrestadorService(ctx);

        // Act
        var resultado = await service.DeletarAsync(999);

        // Assert
        resultado.Should().BeFalse();
    }
}
