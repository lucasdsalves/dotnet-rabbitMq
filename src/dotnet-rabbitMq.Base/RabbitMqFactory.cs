using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace dotnet_rabbitMq.Base
{
    public class RabbitMqFactory : IRabbitMqFactory
    {
        private string rabbitMqUri = "amqp://guest:guest@localhost:5672";
        private string exchangeName = "DemoExchange";
        private string routingKey = "demo-routing-key";
        private string queueName = "DemoQueue";
        private IConnection connection = null;
        private IModel channel = null;

        public RabbitMqFactory()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(rabbitMqUri);
            factory.ClientProvidedName = "Demo App";

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);
        }

        public Task PublishMessage(string message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(message);

            Console.WriteLine($"Sending Message... ");

            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

            Console.WriteLine($"Message sent");

            return Task.CompletedTask;
        }

        public Task ConsumeMessage()
        {
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                var body = args.Body.ToArray();

                string message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Message Received: {message}");

                channel.BasicAck(args.DeliveryTag, false);
            };

            string consumerTag = channel.BasicConsume(queueName, false, consumer);

            Console.ReadLine();

            channel.BasicCancel(consumerTag);

            return Task.CompletedTask;
        }
    }
}
