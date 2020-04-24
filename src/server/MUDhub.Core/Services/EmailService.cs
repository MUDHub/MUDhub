using MUDhub.Core.Abstracts;
using MUDhub.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MUDhub.Core.Services
{
    public class EmailService : IEmailService
    {
        public bool Send(MailMaker mailmaker)
        {
            if (mailmaker != null)
            {
                var email = new MailMessage();
                var sender = new MailAddress(mailmaker.Sender);
                email.From = sender;

                email.To.Add(mailmaker.Receiver);

                email.Subject = mailmaker.Subject;

                email.Body = mailmaker.Message;

                var mailClient = new SmtpClient(mailmaker.Servername, int.Parse(mailmaker.Port));

                var credentials = new NetworkCredential(mailmaker.Username, mailmaker.Password);

                mailClient.Credentials = credentials;

                mailClient.Send(email);
                return true;
            }
            return false;
        }
    }
}
