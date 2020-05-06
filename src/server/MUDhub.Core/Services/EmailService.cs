using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Configurations;
using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class EmailService : IEmailService, IDisposable
    {
        private readonly MailConfiguration _mailConfiguration;
        private readonly ILogger<EmailService>? _logger;
        private readonly SmtpClient _client;

        public EmailService(IOptions<MailConfiguration> mailConfiguration, ILogger<EmailService>? logger = null)
            : this(mailConfiguration?.Value ?? throw new ArgumentNullException(nameof(mailConfiguration)), logger)
        {
        }

        internal EmailService(MailConfiguration configuration, ILogger<EmailService>? logger = null)
        {
            _mailConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
            _client = new SmtpClient(_mailConfiguration.Servername, _mailConfiguration.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_mailConfiguration.Mail, _mailConfiguration.Password)
            };
        }

        /// <summary>
        /// An email with a reset key is sent.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="resetKey"></param>
        /// <returns></returns>
        public async Task<bool> SendAsync(string receiver, string resetKey)
        {
            using MailMessage email = CreateResetMailMessage(receiver, resetKey);
            try
            {
                await _client.SendMailAsync(email)
                    .ConfigureAwait(false);
            }
            catch (SmtpException e)
            {
                _logger?.LogError(e, $"Can't deliver Email to target email address: '{receiver}'.");
                return false;
            }
            return true;
        }

        private MailMessage CreateResetMailMessage(string receiver, string resetKey)
        {
            var sender = new MailAddress(_mailConfiguration.Sender);
            var email = new MailMessage
            {
                From = sender,
                Sender = sender,
                Subject = _mailConfiguration.SubjectReset,
                Body = CreateResetMessage(resetKey)
            };
            email.To.Add(receiver);
            return email;
        }

        private string CreateResetMessage(string resetKey)
        {
            var resetLink = $"http://game.mudhub.de/login/reset?key={resetKey}";
            var message = string.Format(CultureInfo.InvariantCulture, _mailConfiguration.MessageReset, resetLink);
            return message;
        }

        public void Dispose() => _client.Dispose();
    }
}
