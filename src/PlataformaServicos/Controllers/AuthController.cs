using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlataformaServicos.Data;
using PlataformaServicos.DTOs;
using PlataformaServicos.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlataformaServicos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthController(
            IConfiguration configuration,
            AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var usuarioExistente =
                await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (usuarioExistente != null)
            {
                return BadRequest("E-mail já cadastrado.");
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Perfil = dto.Perfil
            };

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Usuário cadastrado com sucesso."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var usuario =
                await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (usuario == null)
            {
                return Unauthorized("Usuário não encontrado.");
            }

            var senhaValida =
                BCrypt.Net.BCrypt.Verify(
                    dto.Senha,
                    usuario.SenhaHash);

            if (!senhaValida)
            {
                return Unauthorized("Senha inválida.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome),

                new Claim(ClaimTypes.Email, usuario.Email),

                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]!));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: credentials);

            return Ok(new
            {
                token =
                    new JwtSecurityTokenHandler()
                        .WriteToken(token),

                perfil = usuario.Perfil,

                nome = usuario.Nome
            });
        }
    }
}