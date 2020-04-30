using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
        public User? User{ get; set; }
    }
}
