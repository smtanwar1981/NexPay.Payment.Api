using Microsoft.EntityFrameworkCore;
using NexPay.Payment.Api.Model;

namespace NexPay.Payment.Api.Context
{
    public class InMemoryDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ContractDb");
        }
        public DbSet<Contract>? Contracts { get; set; }
    }
}
