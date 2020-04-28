using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Configurations
{
    public class MailConfiguration
    {
        public string Sender { get; set; } = "Master@mudhub.de";
        public string SubjectReset { get; set; } = "Passwort zuruecksetzten";
        public string MessageReset { get; set; } = "Hier gehts zum Link {0}";
        public string Username { get; set; } = "Master@mudhub.de";
        public string Password { get; set; } = "";
        public string Servername { get; set; } = "smtp.strato.de";
        public int Port { get; set; } = 465;
    }
}
