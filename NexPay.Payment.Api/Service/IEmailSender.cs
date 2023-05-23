using NexPay.Payment.Api.Email;

namespace NexPay.Payment.Api.Service
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
