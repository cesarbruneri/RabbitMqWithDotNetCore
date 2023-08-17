// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var rabbitMqFactory = new ConnectionFactory { HostName = "localhost" };

using var connection = rabbitMqFactory.CreateConnection();

using var channel = connection.CreateModel();

string exchange = "direct_logs";
string routingKey = "black";

channel.ExchangeDeclare(
    exchange: exchange,
    type: ExchangeType.Direct);

var queueName = channel.QueueDeclare().QueueName;

if (args.Length < 1)
{
    Console.Error.WriteLine(
        "Usage: {0} [info] [warning] [error]",
        Environment.GetCommandLineArgs()[0]);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

foreach (var severity in args)
{
    channel.QueueBind(
        queue: queueName,
        exchange: exchange,
        routingKey: severity);
}

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
    queueName,
    autoAck: true,
    consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();