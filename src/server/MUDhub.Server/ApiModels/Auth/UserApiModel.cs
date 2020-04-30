using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Auth
{
    public class UserApiModel
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}
