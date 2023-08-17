// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

var rabbitMqFactory = new ConnectionFactory { HostName = "localhost" };
using var connection = rabbitMqFactory.CreateConnection();
using var channel = connection.CreateModel();

string exchange = "topic_logs";
var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";

channel.ExchangeDeclare(
    exchange: exchange,
    type: ExchangeType.Topic);

var message = GetMessage(args);
Publish(message);

Console.WriteLine($" [x] Sent '{routingKey}':'{message}'");
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
        exchange: exchange,
        routingKey: routingKey,
        basicProperties: null,
        body: body);
}

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}