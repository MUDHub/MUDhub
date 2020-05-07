using System;
using MUDhub.Core.Models.Rooms;
using MUDhub.Server.ApiModels.Muds.Areas;

namespace MUDhub.Server.ApiModels.Muds.Rooms
{
    public class RoomApiModel
    {
        public string RoomId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageKey { get; set; } = string.Empty;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public AreaApiModel Area { get; set; } = new AreaApiModel();
        public bool IsDefaultRoom { get; set; } = false;

        public static RoomApiModel ConvertFromRoom(Room room)
        {
            if (room is null)
            {
                throw new ArgumentNullException(nameof(room));
            }
            return new RoomApiModel()
            {
                //Area = AreaApiModel.ConvertFromArea(room.Area),
                RoomId = room.Id,
                Name = room.Name,
                Description = room.Description,
                X = room.X,
                Y = room.Y,
                ImageKey = room.ImageKey,
                IsDefaultRoom = room.IsDefaultRoom
            };
        }
    }
}
