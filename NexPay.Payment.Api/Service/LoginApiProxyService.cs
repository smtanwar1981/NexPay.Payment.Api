using NexPay.Payment.Api.Model;

namespace NexPay.Payment.Api.Service
{
    public class LoginApiProxyService : ILoginApiProxyService
    {
        private readonly IConfiguration _configuration;
        public LoginApiProxyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<UserAuthenicationResponse> AuthenticateRequest(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetValue<string>("AuthenticationBaseUrl"));
                var result = await client.GetAsync($"{_configuration.GetValue<string>("AuthenticationBaseUri")}{token}");
                if (result.IsSuccessStatusCode)
                {
                    return new UserAuthenicationResponse { IsAuthenticated = true, UserEmail = result.Content.ReadAsStringAsync().Result };
                }
                else
                    return new UserAuthenicationResponse { IsAuthenticated = false, UserEmail = string.Empty };
            }
        }
    }
}
