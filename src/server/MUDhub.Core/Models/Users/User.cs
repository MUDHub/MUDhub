using MUDhub.Core.Helper;
using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MUDhub.Core.Models.Users
{
    public class User
    {
        public User()
        {
        }

        public User(string id)
        {
            Id = id;
        }

        public string Id { get; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NormalizedEmail { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Roles Role { get; set; } = Roles.Player;

        public string? PasswordResetKey { get; set; }

        //Todo: add Navigation properties
        public virtual ICollection<Character> Characters { get; set; } = new Collection<Character>();
        public virtual ICollection<MudJoinRequest> Joins { get; set; } = new Collection<MudJoinRequest>();
        public virtual ICollection<MudGame> MudGames { get; set; } = new Collection<MudGame>();


        /// <summary>
        /// Checks asynchronously if the user has the role.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(Roles role)
        {
            return UserHelpers.IsUserInRole(this, role);
        }
    }
}
