// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($" [x] Received {message}");

    int dots = message.Split('.').Length - 1;
    Thread.Sleep(dots * 1000);

    Console.WriteLine(" [x] Done");
};

channel.BasicConsume(
    queue,
    autoAck: true,
    consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();