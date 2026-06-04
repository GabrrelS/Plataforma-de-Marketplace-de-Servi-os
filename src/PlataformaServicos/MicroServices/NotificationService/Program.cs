using NotificationService.Hubs;
using NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Serviços
builder.Services.AddControllers();

// SignalR
builder.Services.AddSignalR();

builder.Services.AddScoped<NotificacaoService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("SignalRPolicy");

app.MapControllers();

app.MapHub<PropostaHub>("/hubs/propostas");

app.Run();