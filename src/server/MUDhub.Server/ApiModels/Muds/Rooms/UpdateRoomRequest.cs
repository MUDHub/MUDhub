using MUDhub.Core.Abstracts.Models.Rooms;

namespace MUDhub.Server.ApiModels.Muds.Rooms
{
    public class UpdateRoomRequest
    {
        public string? Description { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? ImageKey { get; set; } = null;
        public bool IsDefaultRoom { get; set; } = false;


        public static UpdateRoomArgs ConvertUpdatesArgs(UpdateRoomRequest requestRoom)
        {
            if (requestRoom is null)
            {
                throw new System.ArgumentNullException(nameof(requestRoom));
            }

            return new UpdateRoomArgs
            {
                Description = requestRoom.Description,
                Name = requestRoom.Name,
                ImageKey = requestRoom.ImageKey,
                IsDefaultRoom = requestRoom.IsDefaultRoom
            };
        }
    }
}
