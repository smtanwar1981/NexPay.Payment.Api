using NexPay.Payment.Api.Model;

namespace NexPay.Payment.Api.Repository
{
    public interface IContractRepository
    {
        /// <summary>
        /// To add a new contract with new status.
        /// </summary>
        /// <param name="request">A contract request.</param>
        /// <returns>A contract status as string.</returns>
        Task<string> AddContract(SubmitContractRequest request);

        /// <summary>
        /// To find a contract by ContractId.
        /// </summary>
        /// <param name="contractId">A contractId string</param>
        /// <returns>An existing contract if found.</returns>
        Task<Contract> FindContractByContractId(string contractId);

        /// <summary>
        /// Update the status of an existing contract.
        /// </summary>
        /// <param name="contract">A contract object to update.</param>
        /// <returns>boolean value true if status update or false if status not updated.</returns>
        Task<Contract> UpdateContractStatus(Contract contract);

        /// <summary>
        /// To delete an existing contract.
        /// </summary>
        /// <param name="contract">A contract object to delete.</param>
        /// <returns>A delete status true if deleted and false if not deleted.</returns>
        Task<bool> DeleteContract(Contract contract);

        /// <summary>
        /// To get list of existing contract.
        /// </summary>
        /// <returns>List of contracts.</returns>
        Task<List<Contract>> GetContractsList();

        /// <summary>
        /// To get list of existing contracts filter by UserId.
        /// </summary>
        /// <param name="userEmail">Unique userEmail.</param>
        /// <returns>Filtered list of contracts.</returns>
        Task<List<Contract>> GetContractsByUserEmail(string userEmail);
    }
}
