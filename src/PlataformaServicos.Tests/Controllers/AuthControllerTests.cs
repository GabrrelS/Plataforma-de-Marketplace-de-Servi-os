using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlataformaServicos.Controllers;
using PlataformaServicos.DTOs;
using PlataformaServicos.Models;
using PlataformaServicos.Tests.Helpers;

namespace PlataformaServicos.Tests.Controllers;

public class AuthControllerTests
{
    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:Key", "chave-super-secreta-para-testes-unitarios-32chars!!" },
                { "Jwt:Issuer", "PlataformaServicos" },
                { "Jwt:Audience", "PlataformaServicos" }
            })
            .Build();

    // ────────────────────────────────────────────────────────────
    //  POST /api/auth/register
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Register_ComDadosValidos_DeveRetornar200()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var controller = new AuthController(BuildConfiguration(), context);
        var dto = new RegisterDto
        {
            Nome = "Maria",
            Email = "maria@email.com",
            Senha = "Senha@123",
            Perfil = "Cliente"
        };

        // Act
        var result = await controller.Register(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Register_DeveHashearASenhaAntesDePersistitr()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var controller = new AuthController(BuildConfiguration(), context);
        var dto = new RegisterDto
        {
            Nome = "Pedro",
            Email = "pedro@email.com",
            Senha = "Senha@456",
            Perfil = "Prestador"
        };

        // Act
        await controller.Register(dto);

        // Assert
        var usuario = context.Usuarios.First();
        usuario.SenhaHash.Should().NotBe("Senha@456");
        BCrypt.Net.BCrypt.Verify("Senha@456", usuario.SenhaHash).Should().BeTrue();
    }

    [Fact]
    public async Task Register_ComEmailDuplicado_DeveRetornar400()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        context.Usuarios.Add(new Usuario
        {
            Nome = "Existente",
            Email = "duplicado@email.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("123"),
            Perfil = "Cliente"
        });
        await context.SaveChangesAsync();

        var controller = new AuthController(BuildConfiguration(), context);
        var dto = new RegisterDto
        {
            Nome = "Novo",
            Email = "duplicado@email.com",
            Senha = "Senha@789",
            Perfil = "Cliente"
        };

        // Act
        var result = await controller.Register(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Register_DevePersistirNoBancoDeDados()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var controller = new AuthController(BuildConfiguration(), context);
        var dto = new RegisterDto
        {
            Nome = "Lucas",
            Email = "lucas@email.com",
            Senha = "Abc@123",
            Perfil = "Prestador"
        };

        // Act
        await controller.Register(dto);

        // Assert
        context.Usuarios.Should().HaveCount(1);
        context.Usuarios.First().Perfil.Should().Be("Prestador");
    }

    // ────────────────────────────────────────────────────────────
    //  POST /api/auth/login
    // ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Login_ComCredenciaisValidas_DeveRetornar200ComToken()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        const string senha = "Senha@Correta";
        context.Usuarios.Add(new Usuario
        {
            Nome = "Julia",
            Email = "julia@email.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
            Perfil = "Cliente"
        });
        await context.SaveChangesAsync();

        var controller = new AuthController(BuildConfiguration(), context);
        var dto = new LoginDto { Email = "julia@email.com", Senha = senha };

        // Act
        var result = await controller.Login(dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var body = okResult.Value!.ToString()!;
        body.Should().Contain("token");
    }

    [Fact]
    public async Task Login_ComEmailInexistente_DeveRetornar401()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        var controller = new AuthController(BuildConfiguration(), context);
        var dto = new LoginDto { Email = "naocadastrado@email.com", Senha = "qualquer" };

        // Act
        var result = await controller.Login(dto);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_ComSenhaIncorreta_DeveRetornar401()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        context.Usuarios.Add(new Usuario
        {
            Nome = "Camila",
            Email = "camila@email.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("SenhaCorreta"),
            Perfil = "Cliente"
        });
        await context.SaveChangesAsync();

        var controller = new AuthController(BuildConfiguration(), context);
        var dto = new LoginDto { Email = "camila@email.com", Senha = "SenhaErrada" };

        // Act
        var result = await controller.Login(dto);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_DeveRetornarPerfilENomeDoUsuario()
    {
        // Arrange
        await using var context = DbContextFactory.CreateInMemory();
        const string senha = "MinhaS3nha!";
        context.Usuarios.Add(new Usuario
        {
            Nome = "Thiago Prestador",
            Email = "thiago@email.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
            Perfil = "Prestador"
        });
        await context.SaveChangesAsync();

        var controller = new AuthController(BuildConfiguration(), context);
        var dto = new LoginDto { Email = "thiago@email.com", Senha = senha };

        // Act
        var result = await controller.Login(dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var body = okResult.Value!.ToString()!;
        body.Should().Contain("Prestador");
        body.Should().Contain("Thiago Prestador");
    }
}
