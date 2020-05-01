using MUDhub.Core.Helper;
using MUDhub.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Auth
{
    public class UserApiModel
    {
        public UserApiModel(User user)
        {
            CreateFromUser(user, this);
        }

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
            return new UserApiModel(user);
        }
        private static void CreateFromUser(User user, UserApiModel model)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            model.Id = user.Id;
            model.Email = user.Email;
            model.FirstName = user.Name;
            model.LastName = user.Lastname;
            model.Roles = UserHelpers.ConvertRoleToList(user.Role);

        }
    }
}
