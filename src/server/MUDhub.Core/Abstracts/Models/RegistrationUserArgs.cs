using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models
{
    public class RegistrationUserArgs : UpdateUserArgs
    {
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null;

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (obj is RegistrationUserArgs rr)
                return rr.Email == Email && rr.Firstname == Firstname && rr.Lastname == Lastname && rr.Password == Password;
            else
                return false;
        }
    }
}
