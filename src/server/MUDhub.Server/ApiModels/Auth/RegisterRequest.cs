using MUDhub.Core.Abstracts.Models;
using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string Firstname { get; set; } = string.Empty;

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
                Firstname = request.Firstname,
                Password = request.Password
            };
        }
    }
}
