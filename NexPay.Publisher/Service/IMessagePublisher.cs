
namespace NexPay.Publisher.Service
{
    public interface IMessagePublisher
    {
        public void PublishMessage<T>(T message);
    }
}
