using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs.Models
{
    public class RoomConnectionSignalRModel
    {
        public string RoomName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Direction Direction { get; set; }


        public static RoomConnectionSignalRModel Convert(RoomConnection connection, Room actualRoom)
        {
            if (connection is null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (actualRoom is null)
            {
                throw new ArgumentNullException(nameof(actualRoom));
            }

            var model = new RoomConnectionSignalRModel();
            if (connection.Room1Id == actualRoom.Id)
            {
                model.RoomName = connection.Room2.Name;
                model.Description = connection.Room2.Description;
            }
            else
            {
                model.RoomName = connection.Room1.Name;
                model.Description = connection.Room1.Description;

            }

            if (connection.Room1.AreaId == connection.Room2.AreaId)
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
                        model.Direction = Direction.South;
                    }
                    break;
                    case (0, 1):
                    {
                        model.Direction = Direction.North;
                    }
                    break;
                    case (-1, 0):
                    {
                        model.Direction = Direction.East;
                    }
                    break;
                    case (1, 0):
                    {
                        model.Direction = Direction.West;
                    }
                    break;
                    default:
                    {
                        model.Direction = Direction.Portal;
                    }
                    break;
                }
            }
            else
            {
                model.Direction = Direction.Portal;
            }


            return model;
        }
    }
}
