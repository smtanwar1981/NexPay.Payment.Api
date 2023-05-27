using Microsoft.AspNetCore.Mvc;

namespace NexPay.Payment.Api.Core
{
    public class AuthenticateRequestAttribute : TypeFilterAttribute
    {
        public AuthenticateRequestAttribute() : base(typeof(AuthenticateRequestFilter))
        {
        }
    }
}
