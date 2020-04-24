using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MUDhub.Core.Services.Models
{
    public class MailMaker
    {
        public string Sender { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Servername { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
    }
}
