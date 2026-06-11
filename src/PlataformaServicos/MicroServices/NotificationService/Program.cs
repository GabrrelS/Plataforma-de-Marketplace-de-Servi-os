using NotificationService.Hubs;
using NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddScoped<NotificacaoService>();

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

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run("http://+:5009");
