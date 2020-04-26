using System;
using System.Collections.Generic;
using System.Text;

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
        public MudGame MudGame { get; set; }

        public string UserId { get; }

        public MudJoinState State { get; set; } = MudJoinState.Requested;


    }
}
