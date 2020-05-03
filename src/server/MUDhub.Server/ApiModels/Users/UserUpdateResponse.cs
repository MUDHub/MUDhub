using MUDhub.Server.ApiModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Users
{
    public class UserUpdateResponse : BaseResponse
    {
        public UserApiModel User { get; set; } = new UserApiModel();

    }
}
