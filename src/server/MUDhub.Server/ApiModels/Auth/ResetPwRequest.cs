using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Auth
{
    public class ResetPwRequest
    {
        [Required]
        public string PasswordResetKey { get; set; } = string.Empty;

        [Required]
        public string NewPasword { get; set; } = string.Empty;
    }
}
