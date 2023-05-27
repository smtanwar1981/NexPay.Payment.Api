using Newtonsoft.Json;
using NexPay.Publisher.Common;
using RabbitMQ.Client;
using System.Text;

namespace NexPay.Publisher.Service
{
    public class MessagePublisher : IMessagePublisher
    {
        public void PublishMessage<T>(T message)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = PublisherConstants.HostName;
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(PublisherConstants.PaymentQueueName, exclusive: false);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: PublisherConstants.PaymentQueueName, body: body);
        }
    }
}
