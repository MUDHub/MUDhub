using MUDhub.Core.Abstracts.Models.Rooms;

namespace MUDhub.Core.Abstracts.Models.Connections
{
    public class RoomConnectionsArgs
    {
        public LockArgs LockArgs { get; set; } = new LockArgs();
        public string Description { get; set; } = string.Empty;
    }
}
