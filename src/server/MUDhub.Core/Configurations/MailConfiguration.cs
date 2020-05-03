using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Configurations
{
    public class MailConfiguration
    {
        public string Sender { get; set; } = string.Empty;
        public string SubjectReset { get; set; } = "Passwort zuruecksetzten";
        public string MessageReset { get; set; } = "Hier gehts zum Link {0}";
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Servername { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
    }
}
