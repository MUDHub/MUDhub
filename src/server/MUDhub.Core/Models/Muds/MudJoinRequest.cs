using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public User User { get; set; }
        public MudJoinState State { get; set; } = MudJoinState.Requested;


    }
}
