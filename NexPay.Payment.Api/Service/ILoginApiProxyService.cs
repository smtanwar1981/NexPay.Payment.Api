namespace NexPay.Payment.Api.Service
{
    public interface ILoginApiProxyService
    {
        public Task<bool> AuthenticateRequest(string token);
    }
}
