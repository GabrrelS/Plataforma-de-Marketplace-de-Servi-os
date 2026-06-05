using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PlataformaServicos.Data;
using PlataformaServicos.Services;
using PlataformaServicos.HttpClients;
using PlataformaServicos.Consumers;
using MassTransit;
using Serilog;
using Serilog.Formatting.Json;
using HealthChecks.UI.Client;
using Prometheus;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(formatter: new JsonFormatter(), path: "logs/app-.json", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Iniciando PlataformaServicos");
    builder.Services.AddControllers();
    
    // --- CORREÇÃO 1: Adicionando o serviço de CORS para liberar o Angular ---
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular", policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Permite o seu Frontend no Docker/Local
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString)
    );

    builder.Services
        .AddHealthChecks()
        .AddNpgSql(connectionString!, name: "postgres");

    builder.Services.AddScoped<PrestadorService>();
    builder.Services.AddScoped<ClienteService>();
    builder.Services.AddScoped<PropostaService>();
    builder.Services.AddMicroserviceClients();
    builder.Services.AddScoped<GatewayService>();

    // Registro do MediatR para o CQRS
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<PropostaCriadaConsumer>();
        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host("localhost", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
            cfg.ReceiveEndpoint("proposta-criada", e =>
            {
                e.ConfigureConsumer<PropostaCriadaConsumer>(ctx);
            });
        });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    
    // --- CORREÇÃO 2: Ativando o CORS logo no início do pipeline ---
    app.UseCors("AllowAngular");
    app.UseRouting();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PlataformaServicos API V1");
        options.RoutePrefix = "swagger";
    });

    app.UseHttpsRedirection();
    app.UseHttpMetrics();
    app.MapMetrics();
    
    app.UseAuthorization();
    app.MapControllers();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

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