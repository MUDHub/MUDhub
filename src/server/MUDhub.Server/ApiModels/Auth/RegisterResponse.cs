using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Auth
{
    public class RegisterResponse : BaseResponse
    {
        public UserApiModel User { get; set; }

    }
}
