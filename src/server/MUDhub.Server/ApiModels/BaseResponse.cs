namespace MUDhub.Server.ApiModels
{
    public class BaseResponse
    {
        public string Errormessage { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = true;
    }
}
