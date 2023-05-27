using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using NexPay.Payment.Api.Service;

namespace NexPay.Payment.Api.Core
{
    public class AuthenticateRequestFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILoginApiProxyService _loginApiProxyService;
        public AuthenticateRequestFilter(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILoginApiProxyService loginApiProxyService)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration; 
            _loginApiProxyService = loginApiProxyService;
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].Any())
            {
                var _bearer_token = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(_bearer_token))
                {
                    context.Result = new UnauthorizedObjectResult(string.Empty);
                    return;
                }
                var authenicationResponse = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
                if (authenicationResponse != null && !authenicationResponse.IsAuthenticated)
                {
                    context.Result = new UnauthorizedObjectResult(string.Empty);
                    return;
                }                
            }
        }
    }
}
