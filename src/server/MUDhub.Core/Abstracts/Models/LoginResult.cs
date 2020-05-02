using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models
{
    public class LoginResult
    {
        public LoginResult(bool succeeded, string? token = null, User? user = null)
        {
            Succeeded = succeeded;
            Token = token;
            User = user;
        }

        public bool Succeeded { get; }
        public string? Token { get; }
        public User? User { get; }
    }
}
