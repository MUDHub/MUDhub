using System;
using System.Collections.Generic;
using System.Linq;
using MUDhub.Core.Models.Connections;
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
        public AreaApiModel Area { get; set; } = new AreaApiModel();
        public bool IsDefaultRoom { get; set; } = false;
        public IEnumerable<ItemInstanceApiModel> ItemInstances { get; set; }
        public Connections Connections { get; set; }

        public static RoomApiModel ConvertFromRoom(Room room)
        {
            if (room is null)
            {
                throw new ArgumentNullException(nameof(room));
            }
            return new RoomApiModel()
            {
                Area = AreaApiModel.ConvertFromArea(room.Area),
                RoomId = room.Id,
                Name = room.Name,
                Description = room.Description,
                X = room.X,
                Y = room.Y,
                ImageKey = room.ImageKey,
                IsDefaultRoom = room.IsDefaultRoom,
                ItemInstances = room.Inventory.ItemInstances.Select(ii => ItemInstanceApiModel.ConvertFromItemInstance(ii)),
                Connections = Connections.CreateFromList(room.Connections)
            };
        }
    }


    public class Connections
    {
        public bool South { get; set; }
        public bool North { get; set; }
        public bool West { get; set; }
        public bool East { get; set; }

        public static Connections CreateFromList(IEnumerable<RoomConnection> connections)
        {
            Connections c = new Connections();
            foreach (var connection in connections)
            {
                var xDif = connection.Room1.X - connection.Room2.X;
                var yDif = connection.Room1.Y - connection.Room2.Y;

                switch ((xDif, yDif))
                {
                    case (0, -1):
                        {
                            c.South = true;
                        }
                        break;
                    case (0, 1):
                        {
                            c.North = true;
                        }
                        break;
                    case (-1, 0):
                        {
                            c.East = true;
                        }
                        break;
                    case (1, 0):
                        {
                            c.West = true;
                        }
                        break;
                }
            }

            return c;
        }
    }
}
