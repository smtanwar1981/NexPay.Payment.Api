using Microsoft.EntityFrameworkCore;
using NexPay.Payment.Api.Common;
using NexPay.Payment.Api.Context;
using NexPay.Payment.Api.Model;

namespace NexPay.Payment.Api.Repository
{
    public class ContractRepository : IContractRepository
    {
        public ContractRepository()
        {
            using (var context = new InMemoryDbContext())
            {
                var contracts = new List<Contract>
                { 
                    new Contract { 
                        ContractId = Guid.NewGuid().ToString(), 
                        ContractStatus = Constants.ContractStatusNew, 
                        ConversionRate = Convert.ToDecimal(0.66), 
                        FromCurrencyCode = Constants.AUDCurrencyCode, 
                        ToCurrencyCode = Constants.USDCurrencyCode, 
                        InitialAmount = 100, 
                        FinalAmount = Convert.ToDecimal(100 * 0.66) 
                    },
                    new Contract {
                        ContractId = Guid.NewGuid().ToString(),
                        ContractStatus = Constants.ContractStatusPending,
                        ConversionRate = Convert.ToDecimal(1.50),
                        FromCurrencyCode = Constants.USDCurrencyCode,
                        ToCurrencyCode = Constants.AUDCurrencyCode,
                        InitialAmount = 100,
                        FinalAmount = Convert.ToDecimal(100 * 1.50)
                    }
                };
                context.Contracts?.AddRange(contracts);
                context.SaveChanges();
            }
        }

        /// <inheritdoc />
        public async Task<string> AddContract(SubmitContractRequest request)
        {
            var newContract = new Contract
            { 
                ContractId = Guid.NewGuid().ToString(),
                ContractStatus = Constants.ContractStatusNew,
                ConversionRate = request.ConversionRate,
                FromCurrencyCode = request.FromCurrencyCode,
                ToCurrencyCode = request.ToCurrencyCode,
                InitialAmount = request.InitialAmount,
                FinalAmount = Convert.ToDecimal(request.InitialAmount * request.ConversionRate)
            };
            using (var context = new InMemoryDbContext())
            {
                context.Add(newContract);
                await context.SaveChangesAsync();
            };
            return newContract.ContractId;
        }

        /// <inheritdoc />
        public async Task<Contract> FindContractByContractId(string contractId)
        {
            Contract? contract = null;
            using (var context = new InMemoryDbContext())
            {
                contract = await context.Contracts?.FirstOrDefaultAsync(x => string.Equals(x.ContractId, contractId, StringComparison.OrdinalIgnoreCase));
            }
            return contract;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateContractStatus(Contract contract)
        {
            bool updateStatus = false;
            using (var context = new InMemoryDbContext())
            {
                context.Contracts?.Update(contract);
                await context.SaveChangesAsync();
                updateStatus = true;
            }
            return updateStatus;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteContract(Contract contract)
        { 
            bool deleteStatus = false;
            using (var context = new InMemoryDbContext())
            {
                context.Contracts.Remove(contract);
                await context.SaveChangesAsync();
                deleteStatus = true;
            }
            return deleteStatus;
        }
    }
}
