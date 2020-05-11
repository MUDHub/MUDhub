using MUDhub.Core.Models.Connections;

namespace MUDhub.Core.Abstracts.Models.Connections
{
    public class ConnectionResult : BaseResult
    {
        public RoomConnection? RoomConnection { get; set; }
    }
}
