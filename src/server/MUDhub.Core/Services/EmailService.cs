using Microsoft.Extensions.Options;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailConfiguration _mailConfiguration;
        public EmailService(IOptions<MailConfiguration> mailConfiguration)
        {
            if (mailConfiguration is null)
            {
                throw new ArgumentNullException(nameof(mailConfiguration));
            }

            _mailConfiguration = mailConfiguration.Value;
        }
        public async Task<bool> SendAsync(string receiver, string resetKey)
        {

            using var email = new MailMessage();
            var sender = new MailAddress(_mailConfiguration.Sender);
            email.From = sender;

            email.To.Add(receiver);

            email.Subject = _mailConfiguration.Subject;

            email.Body = _mailConfiguration.Message;
            //Todo: String Interpolation

            using var mailClient = new SmtpClient(_mailConfiguration.Servername, _mailConfiguration.Port);
            

            var credentials = new NetworkCredential(_mailConfiguration.Username, _mailConfiguration.Password);

            mailClient.Credentials = credentials;

            await mailClient.SendMailAsync(email ).ConfigureAwait(false);
            return true;
        }
    }
}
