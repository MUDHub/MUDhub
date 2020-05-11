using MUDhub.Core.Helper;
using MUDhub.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MUDhub.Server.ApiModels.Auth
{
    public class UserApiModel
    {
        public UserApiModel()
        {

        }

        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();


        public static UserApiModel CreateFromUser(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return new UserApiModel()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.Name,
                LastName = user.Lastname,
                Roles = UserHelpers.ConvertRoleToList(user.Role)
            };
        }
    }
}
