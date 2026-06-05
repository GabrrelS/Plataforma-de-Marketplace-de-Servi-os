using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;
using PlataformaServicos.Services;

namespace PlataformaServicos.Tests;

public class ClienteServiceTests
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

    [Fact(DisplayName = "Listar retorna lista vazia quando não há clientes")]
    public async Task ListarAsync_SemClientes_RetornaListaVazia()
    {
        using var ctx = CriarContexto("cli_listar_vazio");
        var service = new ClienteService(ctx);

        var resultado = await service.ListarAsync();

        resultado.Should().BeEmpty();
    }

    [Fact(DisplayName = "Listar retorna todos os clientes cadastrados")]
    public async Task ListarAsync_ComClientes_RetornaTodos()
    {
        using var ctx = CriarContexto("cli_listar_todos");
        ctx.Clientes.AddRange(
            new Cliente { Nome = "João",  Email = "joao@email.com",  Telefone = "11999990001" },
            new Cliente { Nome = "Maria", Email = "maria@email.com", Telefone = "11999990002" }
        );
        await ctx.SaveChangesAsync();

        var service = new ClienteService(ctx);
        var resultado = await service.ListarAsync();

        resultado.Should().HaveCount(2);
        resultado.Should().Contain(c => c.Nome == "João");
        resultado.Should().Contain(c => c.Nome == "Maria");
    }

    // =====================================================================
    // BUSCAR POR ID
    // =====================================================================

    [Fact(DisplayName = "BuscarPorId retorna cliente quando existe")]
    public async Task BuscarPorIdAsync_ClienteExistente_RetornaCliente()
    {
        using var ctx = CriarContexto("cli_buscar_existente");
        var cliente = new Cliente { Nome = "Pedro", Email = "pedro@email.com", Telefone = "11988880001" };
        ctx.Clientes.Add(cliente);
        await ctx.SaveChangesAsync();

        var service = new ClienteService(ctx);
        var resultado = await service.BuscarPorIdAsync(cliente.Id);

        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be("Pedro");
        resultado.Telefone.Should().Be("11988880001");
    }

    [Fact(DisplayName = "BuscarPorId retorna null quando cliente não existe")]
    public async Task BuscarPorIdAsync_ClienteInexistente_RetornaNull()
    {
        using var ctx = CriarContexto("cli_buscar_inexistente");
        var service = new ClienteService(ctx);

        var resultado = await service.BuscarPorIdAsync(999);

        resultado.Should().BeNull();
    }

    // =====================================================================
    // CRIAR
    // =====================================================================

    [Fact(DisplayName = "Criar persiste cliente no banco e retorna com Id gerado")]
    public async Task CriarAsync_DadosValidos_PersistCliente()
    {
        using var ctx = CriarContexto("cli_criar_valido");
        var service = new ClienteService(ctx);

        var resultado = await service.CriarAsync(new Cliente
        {
            Nome     = "Sofia",
            Email    = "sofia@email.com",
            Telefone = "11977770001"
        });

        resultado.Id.Should().BeGreaterThan(0);
        resultado.Nome.Should().Be("Sofia");
        ctx.Clientes.Should().HaveCount(1);
    }

    // =====================================================================
    // ATUALIZAR
    // =====================================================================

    [Fact(DisplayName = "Atualizar altera dados do cliente existente")]
    public async Task AtualizarAsync_ClienteExistente_RetornaTrue()
    {
        using var ctx = CriarContexto("cli_atualizar_existente");
        var cliente = new Cliente { Nome = "Tiago", Email = "tiago@email.com", Telefone = "11966660001" };
        ctx.Clientes.Add(cliente);
        await ctx.SaveChangesAsync();

        var service = new ClienteService(ctx);
        var atualizado = new Cliente { Nome = "Tiago Silva", Email = "tiago.silva@email.com", Telefone = "11966669999" };

        var resultado = await service.AtualizarAsync(cliente.Id, atualizado);

        resultado.Should().BeTrue();
        var salvo = await ctx.Clientes.FindAsync(cliente.Id);
        salvo!.Nome.Should().Be("Tiago Silva");
        salvo.Telefone.Should().Be("11966669999");
    }

    [Fact(DisplayName = "Atualizar retorna false quando cliente não existe")]
    public async Task AtualizarAsync_ClienteInexistente_RetornaFalse()
    {
        using var ctx = CriarContexto("cli_atualizar_inexistente");
        var service = new ClienteService(ctx);

        var resultado = await service.AtualizarAsync(999, new Cliente { Nome = "X", Email = "x@x.com", Telefone = "0" });

        resultado.Should().BeFalse();
    }

    // =====================================================================
    // DELETAR
    // =====================================================================

    [Fact(DisplayName = "Deletar remove cliente existente e retorna true")]
    public async Task DeletarAsync_ClienteExistente_RetornaTrue()
    {
        using var ctx = CriarContexto("cli_deletar_existente");
        var cliente = new Cliente { Nome = "Vera", Email = "vera@email.com", Telefone = "11955550001" };
        ctx.Clientes.Add(cliente);
        await ctx.SaveChangesAsync();

        var service = new ClienteService(ctx);
        var resultado = await service.DeletarAsync(cliente.Id);

        resultado.Should().BeTrue();
        ctx.Clientes.Should().BeEmpty();
    }

    [Fact(DisplayName = "Deletar retorna false quando cliente não existe")]
    public async Task DeletarAsync_ClienteInexistente_RetornaFalse()
    {
        using var ctx = CriarContexto("cli_deletar_inexistente");
        var service = new ClienteService(ctx);

        var resultado = await service.DeletarAsync(999);

        resultado.Should().BeFalse();
    }
}
