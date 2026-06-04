using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;
using PlataformaServicos.Services;
using Xunit;

namespace PlataformaServicos.Tests
{
    public class PropostaServiceTests
    {
        private AppDbContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Deve_Criar_Proposta()
        {
            var context = CriarContexto();
            var service = new PropostaService(context);

            var proposta = new Proposta
            {
                Titulo = "Site Institucional",
                Descricao = "Desenvolvimento de site",
                Valor = 1500,
                ClienteId = 1,
                PrestadorId = 2
            };

            var resultado = await service.CriarAsync(proposta);

            Assert.NotNull(resultado);
            Assert.Equal("Site Institucional", resultado.Titulo);
        }

        [Fact]
        public async Task Deve_Listar_Propostas()
        {
            var context = CriarContexto();

            context.Propostas.Add(new Proposta
            {
                Titulo = "Projeto 1",
                Descricao = "Teste",
                Valor = 100
            });

            await context.SaveChangesAsync();

            var service = new PropostaService(context);

            var resultado = await service.ListarAsync();

            Assert.Single(resultado);
        }

        [Fact]
        public async Task Deve_Buscar_Proposta_Por_Id()
        {
            var context = CriarContexto();

            var proposta = new Proposta
            {
                Titulo = "Projeto Teste",
                Descricao = "Teste",
                Valor = 100
            };

            context.Propostas.Add(proposta);

            await context.SaveChangesAsync();

            var service = new PropostaService(context);

            var resultado = await service.BuscarPorIdAsync(proposta.Id);

            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task Deve_Atualizar_Proposta()
        {
            var context = CriarContexto();

            var proposta = new Proposta
            {
                Titulo = "Antigo",
                Descricao = "Antiga",
                Valor = 100
            };

            context.Propostas.Add(proposta);

            await context.SaveChangesAsync();

            var service = new PropostaService(context);

            var novaProposta = new Proposta
            {
                Titulo = "Novo",
                Descricao = "Nova descrição",
                Valor = 500,
                Status = "Aceita"
            };

            var resultado = await service.AtualizarAsync(
                proposta.Id,
                novaProposta);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Deletar_Proposta()
        {
            var context = CriarContexto();

            var proposta = new Proposta
            {
                Titulo = "Excluir",
                Descricao = "Teste",
                Valor = 100
            };

            context.Propostas.Add(proposta);

            await context.SaveChangesAsync();

            var service = new PropostaService(context);

            var resultado = await service.DeletarAsync(proposta.Id);

            Assert.True(resultado);
        }
    }
}