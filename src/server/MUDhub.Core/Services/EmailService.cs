using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Configurations;
using MUDhub.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailConfiguration _mailConfiguration;
        public EmailService(IOptions<MailConfiguration> mailConfiguration)
            :this(mailConfiguration?.Value ?? throw new ArgumentNullException(nameof(mailConfiguration)))
        {
           
        }

        internal EmailService(MailConfiguration configuration)
        {
            _mailConfiguration = configuration;
        }
        public async Task<bool> SendAsync(string receiver, string resetKey)
        {

            using var email = new MailMessage();
            var sender = new MailAddress(_mailConfiguration.Sender);
            email.From = sender;

            email.To.Add(receiver);

            email.Subject = _mailConfiguration.SubjectReset;

            email.Body = _mailConfiguration.MessageReset;
            var resetLink = $"http://game.mudhub.de/login/reset?key={resetKey}";
            var message = string.Format(CultureInfo.InvariantCulture, _mailConfiguration.MessageReset, resetLink);
            using var mailClient = new SmtpClient(_mailConfiguration.Servername, _mailConfiguration.Port);

            var credentials = new NetworkCredential(_mailConfiguration.Username, _mailConfiguration.Password);

            mailClient.Credentials = credentials;

            await mailClient.SendMailAsync(email ).ConfigureAwait(false);
            return true;
        }
    }
}
