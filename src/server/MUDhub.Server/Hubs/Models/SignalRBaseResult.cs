namespace MUDhub.Server.Hubs.Models
{
    public class SignalRBaseResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string DisplayMessage { get; set; } = string.Empty;
    }
}
