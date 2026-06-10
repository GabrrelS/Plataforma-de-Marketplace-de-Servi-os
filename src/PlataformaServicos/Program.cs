using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PlataformaServicos.Data;
using PlataformaServicos.Services;
using Serilog;
using Serilog.Formatting.Json;
using HealthChecks.UI.Client;
using Prometheus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
 
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
 
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Digite apenas o token JWT"
            });
    
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
    });
 
    var jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key não configurado.");
 
    builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
 
                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],
 
                ValidAudience =
                    builder.Configuration["Jwt:Audience"],
 
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    )
            };
    });
 
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
 
    app.UseRouting();
 
    // Swagger disponível em todos os ambientes (Development e Production)
    app.UseSwagger();
 
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "PlataformaServicos API V1"
        );
 
        options.RoutePrefix = "swagger";
    });
 
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
 
    // ======================
    // PROMETHEUS
    // ======================
 
    app.UseHttpMetrics();
 
    app.MapMetrics();
 
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