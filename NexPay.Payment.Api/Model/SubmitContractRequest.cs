namespace NexPay.Payment.Api.Model
{
    public class SubmitContractRequest
    {
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
    }
}
