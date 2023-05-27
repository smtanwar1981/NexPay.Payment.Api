using NexPay.Payment.Api.Model;

namespace NexPay.Payment.Api.Service
{
    public interface ILoginApiProxyService
    {
        public Task<UserAuthenicationResponse> AuthenticateRequest(string token);
    }
}
