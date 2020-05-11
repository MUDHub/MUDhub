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
                Connections = Connections.CreateFromList(room.AllConnections, room)
            };
        }
    }


    public class Connections
    {
        public bool South { get; set; }
        public bool North { get; set; }
        public bool West { get; set; }
        public bool East { get; set; }

        public static Connections CreateFromList(IEnumerable<RoomConnection> connections, Room actualRoom)
        {
            if (connections is null)
                throw new ArgumentNullException(nameof(connections));

            if (actualRoom is null)
                throw new ArgumentNullException(nameof(actualRoom));

            Connections c = new Connections();
            foreach (var connection in connections)
            {
                int xDif, yDif;
                if (actualRoom.Id == connection.Room1.Id)
                {
                    xDif = connection.Room1.X - connection.Room2.X;
                    yDif = connection.Room1.Y - connection.Room2.Y;
                }
                else
                {
                    xDif = connection.Room2.X - connection.Room1.X;
                    yDif = connection.Room2.Y - connection.Room1.Y;
                }

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
