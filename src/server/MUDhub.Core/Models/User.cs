using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models
{
    public class User
    {
        public User(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PasswortHash { get; set; }
        public Roles Role { get; set; }
        //public ICollection<Characters> Characters { get; set; }
    }
}
