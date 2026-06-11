using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;

namespace PlataformaServicos.Tests.Helpers;

public static class DbContextFactory
{
    /// <summary>
    /// Cria um AppDbContext isolado em memória para cada teste,
    /// garantindo que não haja estado compartilhado entre testes.
    /// </summary>
    public static AppDbContext CreateInMemory(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
