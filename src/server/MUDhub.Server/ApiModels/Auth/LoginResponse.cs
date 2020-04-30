using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Auth
{
    public class LoginResponse : BaseResponse
    {
        public string? Token { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }
    }
}
