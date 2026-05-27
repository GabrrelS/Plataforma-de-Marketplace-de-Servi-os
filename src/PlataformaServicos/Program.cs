using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PlataformaServicos.Data;
using PlataformaServicos.Services;
using Serilog;
using Serilog.Formatting.Json;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// ======================
// CONFIGURAÇÃO SERILOG
// ======================

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override(
        "Microsoft",
        Serilog.Events.LogEventLevel.Information
    )

    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()

    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}"
    )

    .WriteTo.File(
        formatter: new JsonFormatter(),
        path: "logs/app-.json",
        rollingInterval: RollingInterval.Day
    )

    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Iniciando PlataformaServicos");

    // ======================
    // SERVIÇOS
    // ======================

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    // ======================
    // DATABASE
    // ======================

    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString)
    );

    // ======================
    // HEALTH CHECKS
    // ======================

    builder.Services
        .AddHealthChecks()
        .AddNpgSql(
            connectionString!,
            name: "postgres"
        );

    // ======================
    // SERVICES
    // ======================

    builder.Services.AddScoped<PrestadorService>();
    builder.Services.AddScoped<ClienteService>();
    builder.Services.AddScoped<PropostaService>();


    // ======================
    // BUILD APP
    // ======================

    var app = builder.Build();

    // ======================
    // MIDDLEWARES
    // ======================

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    // ======================
    // ROTAS
    // ======================

    app.MapControllers();

    app.MapHealthChecks(
        "/health",
        new HealthCheckOptions
        {
            ResponseWriter =
                UIResponseWriter.WriteHealthCheckUIResponse
        }
    );

    Log.Information("Aplicação iniciada");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação falhou ao iniciar");
}
finally
{
    Log.CloseAndFlush();
}