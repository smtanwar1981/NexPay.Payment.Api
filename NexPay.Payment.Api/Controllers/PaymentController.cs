using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NexPay.Payment.Api.Core;
using NexPay.Payment.Api.Email;
using NexPay.Payment.Api.Model;
using NexPay.Payment.Api.Service;
using NexPay.Publisher.Common;
using NexPay.Publisher.Service;

namespace NexPay.Payment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        public readonly ILogger<PaymentController> _logger;
        private readonly ILoginApiProxyService _loginApiProxyService;
        private readonly IContractService _contractService;
        private readonly IMessagePublisher _messagePublisher;
        public PaymentController(ILogger<PaymentController> logger, ILoginApiProxyService loginApiProxyService, IContractService contractService, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _loginApiProxyService = loginApiProxyService;
            _contractService = contractService;
            _messagePublisher = messagePublisher;
        }

        [HttpGet("getContracts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Contract>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetContractsList()
        {
            _logger.LogInformation($"Begin executing GetContractsList() of {nameof(PaymentController)} class.");

            var contractList = new List<Contract>();
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var authenicationResponse = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
            if (authenicationResponse.IsAuthenticated)
            {
                contractList = await _contractService.GetContractsList();
            }
            else
            {
                throw new Exception("Unauthorized access.");
            }

            _logger.LogInformation($"Finish executing GetContractsList() of {nameof(PaymentController)} class.");
            return Ok(contractList);
        }

        [HttpGet("getContractsByUserEmail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Contract>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetContractsListByUserEmail(string userEmail)
        {
            _logger.LogInformation($"Begin executing GetContractsListByUserId() of {nameof(PaymentController)} class.");

            var contractList = new List<Contract>();
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var authenicationResponse = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
            if (authenicationResponse.IsAuthenticated)
            {
                contractList = await _contractService.GetContractsListByUserEmail(userEmail);
            }
            else
            {
                throw new Exception("Unauthorized access.");
            }

            _logger.LogInformation($"Finish executing GetContractsListByUserId() of {nameof(PaymentController)} class.");
            return Ok(contractList);
        }

        [HttpPost("submitContract")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Object))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Object>> SubmitContract(SubmitContractRequest request)
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
            UserAuthenicationResponse authenicationResponse = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
            if (authenicationResponse.IsAuthenticated)
            {
                submitContractId = await _contractService.SubmitContact(request, authenicationResponse.UserEmail);
                _logger.LogInformation($"Finish executing SubmitContract() of {nameof(PaymentController)} class.");
            }
            else
            {
                throw new Exception("Unauthorized access.");
            }            
            return Ok(new { contractId = submitContractId });
        }

        [HttpPost("updateContract")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateContractStatus(UpdateContractRequest request)
        {
            _logger.LogInformation($"Begin executing SubmitContract() of {nameof(PaymentController)} class.");

            if (string.IsNullOrEmpty(request.ContractId))
            {
                return BadRequest($"{nameof(request.ContractId)} can not be null or empty.");
            }
            if (string.IsNullOrEmpty(request.ContractStatus))
            {
                return BadRequest($"{nameof(request.ContractStatus)} can not be null or empty.");
            }

            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var authenicationResponse = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
            bool updateContractStatus = false;
            if (authenicationResponse.IsAuthenticated)
            {
                updateContractStatus = await _contractService.UpdateContractStatus(request.ContractId, request.ContractStatus);
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
            var authenicationResponse = await _loginApiProxyService.AuthenticateRequest(_bearer_token);
            bool deleteContractStatus = false;
            if (authenicationResponse.IsAuthenticated)
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
