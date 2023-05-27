namespace NexPay.Payment.Api.Model
{
    public class Contract
    {
        /// <summary>
        /// Gets or sets ContractId.
        /// </summary>
        public string? ContractId { get; set; }

        /// <summary>
        /// Gets or sets FromCurrencyCode.
        /// </summary>
        public string? FromCurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets ToCurrencyCode.
        /// </summary>
        public string? ToCurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets ConversionRate.
        /// </summary>
        public decimal? ConversionRate { get; set; }

        /// <summary>
        /// Gets or sets InitialAmount.
        /// </summary>
        public decimal? InitialAmount { get; set; }

        /// <summary>
        /// Gets or sets FinalAmount.
        /// </summary>
        public decimal? FinalAmount { get; set; }

        /// <summary>
        /// Gets or sets ContractStatus.
        /// </summary>
        public string? ContractStatus { get; set; }

        /// <summary>
        /// Gets or sets UserEmail.
        /// </summary>
        public string? UserEmail { get; set; }
    }
}
