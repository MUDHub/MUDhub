using MUDhub.Core.Models.Users;

namespace MUDhub.Core.Models.Muds
{
    public class MudJoinRequest
    {


        public MudJoinRequest(string mudId, string userId)
        {
            MudId = mudId;
            UserId = userId;
        }

        public string MudId { get; }
        public virtual MudGame MudGame { get; set; }
        public string UserId { get; }
        public virtual User User { get; set; }
        public MudJoinState State { get; set; } = MudJoinState.Requested;


    }
}
