using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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

Console.WriteLine("Aguardando mensagens...");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine("Pedido recebido:");
    Console.WriteLine(message);

    Console.WriteLine("Email enviado!");
};

channel.BasicConsume(
    queue: "pedido_criado",
    autoAck: true,
    consumer: consumer
);

Console.ReadLine();