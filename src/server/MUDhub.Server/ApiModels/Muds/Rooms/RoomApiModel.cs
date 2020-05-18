using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Rooms;
using MUDhub.Server.ApiModels.Items;
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
        public AreaApiModel? Area { get; set; }
        public bool IsDefaultRoom { get; set; } = false;
        public IEnumerable<ItemInstanceApiModel>? ItemInstances { get; set; }
        public ConnectionsApiModel? Connections { get; set; }
        public string EnterMessage { get; set; } = string.Empty;

        public static RoomApiModel ConvertFromRoom(Room room, bool depth = true)
        {
            if (room is null)
            {
                throw new ArgumentNullException(nameof(room));
            }
            return new RoomApiModel()
            {
                Area = depth ? AreaApiModel.ConvertFromArea(room.Area) : null,
                RoomId = room.Id,
                Name = room.Name,
                Description = room.Description,
                X = room.X,
                Y = room.Y,
                ImageKey = room.ImageKey,
                EnterMessage = room.EnterMessage,
                IsDefaultRoom = room.IsDefaultRoom,
                ItemInstances = depth ? room.Inventory.ItemInstances.Select(ii => ItemInstanceApiModel.ConvertFromItemInstance(ii)) : null,
                Connections = depth ? ConnectionsApiModel.CreateFromList(room.AllConnections, room) : null
            };
        }
    }
}
