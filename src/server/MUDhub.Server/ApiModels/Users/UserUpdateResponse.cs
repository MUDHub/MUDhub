using MUDhub.Server.ApiModels.Auth;

namespace MUDhub.Server.ApiModels.Users
{
    public class UserUpdateResponse : BaseResponse
    {
        public UserApiModel User { get; set; } = new UserApiModel();

    }
}
