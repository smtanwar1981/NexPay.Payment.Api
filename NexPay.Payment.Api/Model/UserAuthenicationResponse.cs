namespace NexPay.Payment.Api.Model
{
    public class UserAuthenicationResponse
    {
        /// <summary>
        /// Gets or sets IsAuthenticated.
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        public string? UserEmail { get; set; }
    }
}
