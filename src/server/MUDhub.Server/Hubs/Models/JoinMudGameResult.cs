using Microsoft.AspNetCore.Identity;
using MUDhub.Server.ApiModels.Muds.Rooms;

namespace MUDhub.Server.Hubs.Models
{
    public class JoinMudGameResult : SignalRBaseResult
    {
        public RoomApiModel? Room { get; set; }
        public string? AreaId { get; set; }
    }
}
