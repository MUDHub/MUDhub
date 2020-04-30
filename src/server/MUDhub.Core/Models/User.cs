using System;
using System.Collections.Generic;
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
        public string PasswordHash { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Player;

        public string? PasswordResetKey { get; set; }

        //Todo: add Navigation properties
        //public ICollection<Characters> Characters { get; set; }
    }
}
