using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/orders", (object order) =>
{
    var factory = new ConnectionFactory()
    {
        HostName = "localhost"
    };

    var connection = factory.CreateConnection();

    var channel = connection.CreateModel();

    channel.QueueDeclare(
        queue: "pedido_criado",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null
    );

    var message = JsonSerializer.Serialize(order);

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "",
        routingKey: "pedido_criado",
        basicProperties: null,
        body: body
    );

    Console.WriteLine("Evento publicado!");

    return Results.Ok(new
    {
        message = "Pedido criado"
    });
});

app.Run("http://localhost:5001");