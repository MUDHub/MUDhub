using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models
{
    public class RegisterResult
    {

        public RegisterResult(bool succeeded, bool usernameAlreadyExists = false, LoginResult? loginResult = null)
        {
            Succeeded = succeeded;
            UsernameAlreadyExists = usernameAlreadyExists;
            LoginResult = loginResult;
        }
        public bool Succeeded { get; }
        public bool UsernameAlreadyExists { get; }
        public LoginResult? LoginResult { get; }

    }
}
