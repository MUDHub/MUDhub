namespace MUDhub.Core.Abstracts.Models
{
    public class BaseResult
    {
        public bool Success { get; set; } = true;
        public string Errormessage { get; set; } = string.Empty;
        public string DisplayMessage { get; set; } = string.Empty;
    }
}
