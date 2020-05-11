using MUDhub.Core.Abstracts.Models.Connections;

namespace MUDhub.Server.ApiModels.Muds.RoomConnections
{
    public class UpdateConnectionRequest
    {
        public string? Description { get; set; } = null;

        public static UpdateRoomConnectionsArgs ConvertUpdatesArgs(UpdateConnectionRequest requestConnection)
        {
            if (requestConnection is null)
            {
                throw new System.ArgumentNullException(nameof(requestConnection));
            }

            return new UpdateRoomConnectionsArgs
            {
                Description = requestConnection.Description
            };
        }
    }
}
