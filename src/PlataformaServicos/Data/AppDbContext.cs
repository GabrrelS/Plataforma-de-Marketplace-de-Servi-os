using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Models;

namespace PlataformaServicos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Prestador> Prestadores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
    }
}