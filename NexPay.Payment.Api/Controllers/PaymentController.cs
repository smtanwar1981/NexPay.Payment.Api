using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NexPay.Payment.Api.Email;
using NexPay.Payment.Api.Model;
using NexPay.Payment.Api.Service;

namespace NexPay.Payment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        public readonly ILogger<PaymentController> _logger;
        public readonly IEmailSender _emailSender;
        private readonly ILoginApiProxyService _loginApiProxyService;
        private readonly IContractService _contractService;
        public PaymentController(ILogger<PaymentController> logger, IEmailSender emailSender, ILoginApiProxyService loginApiProxyService, IContractService contractService)
        {
            _logger = logger;
            _emailSender = emailSender;
            _loginApiProxyService = loginApiProxyService;
            _contractService = contractService;
        }

        [HttpPost("submitContract")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> SubmitContract(SubmitContractRequest request)
        {
            _logger.LogInformation($"Begin executing SubmitContract() of {nameof(PaymentController)} class.");

            if (request == null)
            {
                return BadRequest("Request can not be null or empty");
            }
            if (request.InitialAmount == null || request.InitialAmount <= 0)
            {
                return BadRequest($"{nameof(request.InitialAmount)} can not be null or 0.");
            }
            if (string.IsNullOrEmpty(request.FromCurrencyCode))
            {
                return BadRequest($"{nameof(request.FromCurrencyCode)} can not be null or empty.");
            }
            if (string.IsNullOrEmpty(request.ToCurrencyCode))
            {
                return BadRequest($"{nameof(request.ToCurrencyCode)} can not be null or empty.");
            }
            if (request.ConversionRate == null || request.ConversionRate <= 0)
            {
                return BadRequest($"{request.ConversionRate} can not be null or 0.");
            }

            string submitContractId = string.Empty;
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var isAuthenticated = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
            if (isAuthenticated)
            {
                submitContractId = await _contractService.SubmitContact(request);
                _logger.LogInformation($"Finish executing SubmitContract() of {nameof(PaymentController)} class.");
            }
            else
            {
                throw new Exception("Unauthorized access.");
            }            
            return Ok(submitContractId);

            //var rng = new Random();
            //var message = new Message(new string[] { "challengenexpayemail@gmail.com" }, "Test email", "This is the content from our email.");
            //_emailSender.SendEmail(message);
        }

        [HttpGet("updateContract/{contractId}/{contractStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateContractStatus(string contractId, string contractStatus)
        {
            _logger.LogInformation($"Begin executing SubmitContract() of {nameof(PaymentController)} class.");

            if (string.IsNullOrEmpty(contractId))
            {
                return BadRequest($"{nameof(contractId)} can not be null or empty.");
            }
            if (string.IsNullOrEmpty(contractStatus))
            {
                return BadRequest($"{nameof(contractStatus)} can not be null or empty.");
            }

            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var isAuthenticated = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
            bool updateContractStatus = false;
            if (isAuthenticated)
            {
                updateContractStatus = await _contractService.UpdateContractStatus(contractId, contractStatus);
                _logger.LogInformation($"Finish executing SubmitContract() of {nameof(PaymentController)} class.");
            }
            else
            {
                throw new Exception("Unauthorized access.");
            }

            return Ok(updateContractStatus);
        }

        [HttpDelete("deleteContract")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteContract(string contractId)
        {
            _logger.LogInformation($"Begin executing DeleteContract() of {nameof(PaymentController)} class.");

            if (string.IsNullOrEmpty(contractId))
            {
                return BadRequest($"{nameof(contractId)} can not be null or empty.");
            }

            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var isAuthenticated = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
            bool deleteContractStatus = false;
            if (isAuthenticated)
            {
                deleteContractStatus = await _contractService.DeleteContractByContractId(contractId);
                _logger.LogInformation($"Finish executing SubmitContract() of {nameof(PaymentController)} class.");
            }
            else
            {
                throw new Exception("Unauthorized access.");
            }

            return Ok(deleteContractStatus);
        }
    }
}
