using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using MUDhub.Core.Abstracts.Models;

namespace MUDhub.Server.ApiModels.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

       

        public static RegistrationUserArgs ConvertFromRequest(RegisterRequest request)
        {
            return new RegistrationUserArgs()
            {
                Email = request.Email,
                Lastname = request.Lastname,
                Firstname = request.FirstName,
                Password = request.Password
            };
        }
    }
}
