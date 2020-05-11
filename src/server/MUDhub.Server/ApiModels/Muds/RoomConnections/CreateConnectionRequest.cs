using MUDhub.Core.Abstracts.Models.Connections;
using MUDhub.Core.Models.Connections;
using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Muds.RoomConnections
{
    public class CreateConnectionRequest
    {
        [Required]
        public string RoomId1 { get; set; } = string.Empty;
        [Required]
        public string RoomId2 { get; set; } = string.Empty;
        [Required]
        public LockType LockType { get; set; }

        public string Description { get; set; } = string.Empty;

        public string LockDescription { get; set; } = string.Empty;
        public string LockAssociatedId { get; set; } = string.Empty;

        public static RoomConnectionsArgs ConvertFromRequest(CreateConnectionRequest request)
        {
            if (request is null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            return new RoomConnectionsArgs()
            {
                Description = request.Description,
                LockArgs = new LockArgs()
                {
                    LockType = request.LockType,
                    LockAssociatedId = request.LockAssociatedId,
                    LockDescription = request.LockDescription
                }
            };
        }
    }
}
