using MUDhub.Core.Models.Users;

namespace MUDhub.Core.Abstracts.Models
{
    public class LoginResult : BaseResult
    {
        public string? Token { get; set; }
        public User? User { get; set; }
    }
}
