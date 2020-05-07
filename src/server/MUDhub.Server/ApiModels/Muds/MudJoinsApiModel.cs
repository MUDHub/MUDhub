using MUDhub.Core.Models.Muds;
using System;

namespace MUDhub.Server.ApiModels.Muds
{
    public class MudJoinsApiModel
    {
        public string MudGameId { get; set; } = string.Empty;
        public string MudGameName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Userid { get; set; } = string.Empty;
        public MudJoinState State { get; set; }

        public static MudJoinsApiModel CreateFromJoin(MudJoinRequest joinRequest)
        {
            if (joinRequest is null)
                throw new ArgumentNullException(nameof(joinRequest));

            return new MudJoinsApiModel
            {
                MudGameId = joinRequest.MudId,
                MudGameName = joinRequest.MudGame.Name,
                UserEmail = joinRequest.User.Email,
                Userid = joinRequest.UserId,
                State = joinRequest.State
            };
        }
    }
}
