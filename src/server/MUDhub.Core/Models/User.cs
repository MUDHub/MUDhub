using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUDhub.Core.Models
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
        public ICollection<Character> Characters { get; set; } = new Collection<Character>();
        public ICollection<MudJoinRequest> Joins { get; set; } = new Collection<MudJoinRequest>();
        public ICollection<MudGame> MudGames { get; set; } = new Collection<MudGame>();
    }
}
