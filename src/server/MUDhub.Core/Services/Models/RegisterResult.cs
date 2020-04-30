using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Services.Models
{
    public class RegisterResult
    {

        public RegisterResult(bool succeeded, bool usernameAlreadyExists = false)
        {
            Succeeded = succeeded;
            UsernameAlreadyExists = usernameAlreadyExists;
        }
        public bool Succeeded { get; }
        public bool UsernameAlreadyExists { get; }

    }
}
