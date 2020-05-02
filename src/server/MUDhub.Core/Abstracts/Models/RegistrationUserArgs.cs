using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models
{
    public class RegistrationUserArgs : UpdateUserArgs
    {
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null;
    }
}
