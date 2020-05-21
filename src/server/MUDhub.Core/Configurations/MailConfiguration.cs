namespace MUDhub.Core.Configurations
{
    public class MailConfiguration
    {
        public string Sender { get; set; } = string.Empty;
        public string SubjectReset { get; set; } = "MUDhub: Passwort zuruecksetzten";
        public string MessageReset { get; set; } = "Du hast dein Passwort vergessen? Kein Problem! Folge einfach dem Link und du kannst dir ein neues Passwort aussuchen => {0}";
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Servername { get; set; } = string.Empty;
        public int Port { get; set; }
        public string HostingUrl { get; set; } = string.Empty;
    }
}
