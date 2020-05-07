using MUDhub.Core.Abstracts.Models;
using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Users
{
    public class UserUpdateRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public static UpdateUserArgs ConvertToUserArgs(UserUpdateRequest request)
        {
            return new UpdateUserArgs
            {
                Firstname = request.FirstName,
                Lastname = request.Lastname
            };
        }

    }
}
