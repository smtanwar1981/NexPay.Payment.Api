using NexPay.Payment.Api.Common;
using NexPay.Payment.Api.Model;
using NexPay.Payment.Api.Repository;

namespace NexPay.Payment.Api.Service
{
    public class ContractService : IContractService
    {
        private readonly ILogger<ContractService> _logger;
        private readonly IContractRepository _contractRepository;
        public ContractService(ILogger<ContractService> logger, IContractRepository contractReposiroty)
        {
            _logger = logger;
            _contractRepository = contractReposiroty;
        }

        /// <inheritdoc />
        public async Task<string> SubmitContact(SubmitContractRequest request)
        {
            _logger.LogInformation($"Begin Executing SubmitContract() method of {nameof(ContractService)} class.");

            if (request == null)
            { 
                throw new ArgumentNullException(nameof(request));
            }
            if (string.IsNullOrEmpty(request.FromCurrencyCode))
            {
                throw new ArgumentNullException($"{nameof(request.FromCurrencyCode)} can not be null or empty.");
            }
            if (string.IsNullOrEmpty(request.ToCurrencyCode))
            {
                throw new ArgumentNullException($"{nameof(request.ToCurrencyCode)} can not be null or empty.");
            }
            if (request.InitialAmount == null || request.InitialAmount <= 0)
            {
                throw new ArgumentException($"{nameof(request.InitialAmount)} can not be null, 0 or less than 0.");
            }
            if (request.ConversionRate == null || request.ConversionRate <= 0)
            {
                throw new ArgumentException($"{nameof(request.ConversionRate)} can not be null, 0 or less than 0.");
            }

            string contractId = await _contractRepository.AddContract(request);

            if (!string.IsNullOrEmpty(contractId))
            { 
                /// TBD: Send Email to receipient and Admin
            }
            _logger.LogInformation($"Finish Executing SubmitContract() method of {nameof(ContractService)} class.");

            return contractId;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateContractStatus(string contractId, string contractStatus)
        {
            _logger.LogInformation($"Begin Executing UpdateContractStatus() method of {nameof(ContractService)} class.");

            bool updateContractStatus = false;

            if (string.IsNullOrEmpty(contractId))
            { 
                throw new ArgumentNullException(nameof(contractId));
            }
            if (string.IsNullOrEmpty(contractStatus))
            {
                throw new ArgumentNullException(nameof(contractStatus));
            }
            if (!isContractStatusValid(contractStatus))
            {
                throw new ArgumentException("Invalid contract status, unable to update contract.");
            }

            var contract = await _contractRepository.FindContractByContractId(contractId);
            if (contract == null)
            {
                throw new ArgumentException($"No contract with this contract id - {contractId} found in the system");
            }
            contract.ContractStatus = contractStatus;
            updateContractStatus = await _contractRepository.UpdateContractStatus(contract);

            if (updateContractStatus)
            {
                /// TBD: Send Email to receipient and Admin
            }

            _logger.LogInformation($"Finish Executing UpdateContractStatus() method of {nameof(ContractService)} class.");
            return updateContractStatus;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteContractByContractId(string contractId)
        {
            _logger.LogInformation($"Begin Executing DeleteContractByContractId() method of {nameof(ContractService)} class.");

            bool deleteContractStatus = false;

            if (string.IsNullOrEmpty(contractId))
            {
                throw new ArgumentNullException(nameof(contractId));
            }
            var contract = await _contractRepository.FindContractByContractId(contractId);
            if (contract == null)
            {
                throw new ArgumentException($"No contract with this contract id - {contractId} found in the system");
            }
            deleteContractStatus = await _contractRepository.DeleteContract(contract);
            if (deleteContractStatus)
            {
                /// TBD: Send Email to receipient and Admin
            }

            _logger.LogInformation($"Finish Executing DeleteContractByContractId() method of {nameof(ContractService)} class.");
            return deleteContractStatus;
        }

        private bool isContractStatusValid(string contractStatus)
        {
            bool isValidContractStatus = false;
            switch (contractStatus)
            {
                case Constants.ContractStatusNew: isValidContractStatus = true; break ;
                case Constants.ContractStatusPending: isValidContractStatus = true; break ;
                case Constants.ContractStatusClosed: isValidContractStatus = true; break ;  
                case Constants.ContractStatusApproved: isValidContractStatus = true; break ;    
                case Constants.ContractStatusRejected: isValidContractStatus = true; break ;
            }
            return isValidContractStatus;
        }
    }
}
