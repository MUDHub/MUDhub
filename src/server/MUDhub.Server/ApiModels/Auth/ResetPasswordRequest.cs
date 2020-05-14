using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public string PasswordResetKey { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
