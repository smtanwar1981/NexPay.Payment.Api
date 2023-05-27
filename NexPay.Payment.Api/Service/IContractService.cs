using NexPay.Payment.Api.Model;

namespace NexPay.Payment.Api.Service
{
    public interface IContractService
    {
        /// <summary>
        /// A service method to add contract in the in-memory database.
        /// </summary>
        /// <param name="request">Submit contract request object.</param>
        /// <returns>A contract id string.</returns>
        Task<string> SubmitContact(SubmitContractRequest request, string userEmail);

        /// <summary>
        /// A service method to update the status of the contract.
        /// </summary>
        /// <param name="contractId">A contract Id.</param>
        /// <param name="contractStatus">Status of the contract to update.</param>
        /// <returns>A boolean value as the update status of the contract.</returns>
        Task<bool> UpdateContractStatus(string contractId, string contractStatus);

        /// <summary>
        /// A service method to delete an existing contract from the system.
        /// </summary>
        /// <param name="contractId">Contract id.</param>
        /// <returns>Delete status of the contract.</returns>
        Task<bool> DeleteContractByContractId(string contractId);

        /// <summary>
        /// A method that will return list of existing contracts in the system.
        /// </summary>
        /// <returns>List of contracts.</returns>
        Task<List<Contract>> GetContractsList();

        /// <summary>
        /// A method that will return list of existing contracts filter by UserId.
        /// </summary>
        /// <param name="userId">Unique user Id.</param>
        /// <returns>List of filtered contracts.</returns>
        Task<List<Contract>> GetContractsListByUserEmail(string userEmail);
    }
}
