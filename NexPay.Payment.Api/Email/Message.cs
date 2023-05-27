using MailKit.Net.Smtp;
using MimeKit;

namespace NexPay.Payment.Api.Email
{
    public class Message
    {
        /// <summary>
        /// Gets or sets To.
        /// </summary>
        public List<MailboxAddress> To { get; set; }

        /// <summary>
        /// Gets or sets Subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets Content.
        /// </summary>
        public string Content { get; set; }
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Content = content;
        }
    }
}
