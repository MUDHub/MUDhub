using MUDhub.Core.Models.Users;

namespace MUDhub.Core.Abstracts.Models
{
    public class RegisterResult
    {

        public RegisterResult(bool succeeded, bool usernameAlreadyExists = false, User? user = null)
        {
            Succeeded = succeeded;
            UsernameAlreadyExists = usernameAlreadyExists;
            User = user;
        }
        public bool Succeeded { get; }
        public bool UsernameAlreadyExists { get; }
        public User? User { get; set; }
    }
}
