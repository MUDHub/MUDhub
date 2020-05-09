using MUDhub.Core.Models.Users;

namespace MUDhub.Core.Abstracts.Models
{
    public class RegisterResult : BaseResult
    {
        public bool UsernameAlreadyExists { get; set; }
        public User? User { get; set; }
    }
}
