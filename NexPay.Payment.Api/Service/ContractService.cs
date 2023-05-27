using NexPay.Payment.Api.Common;
using NexPay.Payment.Api.Model;
using NexPay.Payment.Api.Repository;
using NexPay.Publisher.Common;
using NexPay.Publisher.Service;

namespace NexPay.Payment.Api.Service
{
    public class ContractService : IContractService
    {
        private readonly ILogger<ContractService> _logger;
        private readonly IContractRepository _contractRepository;
        private readonly IMessagePublisher _messagePublisher;
        public ContractService(ILogger<ContractService> logger, IContractRepository contractReposiroty, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _contractRepository = contractReposiroty;
            _messagePublisher = messagePublisher;
        }

        /// <inheritdoc />
        public async Task<string> SubmitContact(SubmitContractRequest request, string userEmail)
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
                _messagePublisher.PublishMessage(GetContractSubmissionMessageToPublish(request, contractId, userEmail));
            }
            _logger.LogInformation($"Finish Executing SubmitContract() method of {nameof(ContractService)} class.");

            return contractId;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateContractStatus(string contractId, string contractStatus)
        {
            _logger.LogInformation($"Begin Executing UpdateContractStatus() method of {nameof(ContractService)} class.");

            Contract updatedContract = null;
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
            updatedContract = await _contractRepository.UpdateContractStatus(contract);

            if (updatedContract != null)
            {
                updateContractStatus = true;
                _messagePublisher.PublishMessage(GetContractStatusChangeMessageToPublish(updatedContract));
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

        /// <inheritdoc />
        public async Task<List<Contract>> GetContractsListByUserEmail(string userEmail)
        {
            _logger.LogInformation($"Begin Executing GetContractsList() method of {nameof(ContractService)} class.");

            var contract = await _contractRepository.GetContractsByUserEmail(userEmail);
            if (contract == null)
            {
                throw new Exception($"No contract found in the system");
            }

            _logger.LogInformation($"Finish Executing GetContractsList() method of {nameof(ContractService)} class.");
            return contract;
        }

        /// <inheritdoc />
        public async Task<List<Contract>> GetContractsList() 
        {
            _logger.LogInformation($"Begin Executing GetContractsList() method of {nameof(ContractService)} class.");

            var contract = await _contractRepository.GetContractsList();
            if (contract == null)
            {
                throw new Exception($"No contract found in the system");
            }

            _logger.LogInformation($"Finish Executing GetContractsList() method of {nameof(ContractService)} class.");
            return contract;
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

        private SubmitContractMessagePayload GetContractSubmissionMessageToPublish(SubmitContractRequest request, string contractId, string userEmail)
        {
            return new SubmitContractMessagePayload
            { 
                PayloadId = Guid.NewGuid().ToString(),
                ContractId = contractId,
                UserEmail = userEmail,
                FromCurrencyCode = request.FromCurrencyCode,
                ToCurrencyCode = request.ToCurrencyCode,
                ConversionRate = request.ConversionRate,
                InitialAmount = request.InitialAmount,
                FinalAmount = request.FinalAmount,
                ContractStatus = request.ContractStatus,
            };
        }

        private SubmitContractMessagePayload GetContractStatusChangeMessageToPublish(Contract contract)
        {
            return new SubmitContractMessagePayload
            {
                PayloadId = Guid.NewGuid().ToString(),
                ContractId = contract.ContractId,
                UserEmail = contract.UserEmail,
                FromCurrencyCode = contract.FromCurrencyCode,
                ToCurrencyCode = contract.ToCurrencyCode,
                ConversionRate = contract.ConversionRate,
                InitialAmount = contract.InitialAmount,
                FinalAmount = contract.FinalAmount,
                ContractStatus = contract.ContractStatus,
            };
        }
    }
}
