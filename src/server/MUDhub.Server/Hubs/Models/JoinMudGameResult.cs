using Microsoft.AspNetCore.Identity;

namespace MUDhub.Server.Hubs.Models
{
    public class JoinMudGameResult : SignalRBaseResult
    {
        public string? RoomId { get; set; }
        public string? AreaId { get; set; }
    }
}
