// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

var rabbitMqFactory = new ConnectionFactory { HostName = "localhost" };

using var connection = rabbitMqFactory.CreateConnection();

using var channel = connection.CreateModel();

string queue = "hello";

channel.QueueDeclare(
    queue: queue,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

string? message = string.Empty;
bool stop = true;
do
{
    Console.WriteLine("Digita algo para ser enviado:");
    message = Console.ReadLine();

    Console.WriteLine($" [x] Sent {message}");
    Publish(message);

    stop = SendNewMessage();
} while(stop) ;


Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();

void Publish(string? message)
{
    string messageToSend = string.Empty;
    if (string.IsNullOrWhiteSpace(message) && message == string.Empty)
    {
        messageToSend = "Hello, World!";
    }

    messageToSend = message;

    var body = Encoding.UTF8.GetBytes(messageToSend);

    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: queue,
        basicProperties: null,
        body: body);
}

bool SendNewMessage()
{
    Console.WriteLine("Deseja enviar uma nova mensagem?");
    Console.WriteLine("[0] - Não");
    Console.WriteLine("[1] - Sim");
    var response = Console.ReadLine();

    return response?.Trim() switch
    {
        "0" => false,
        "1" => true,
        _ => false,
    };
}