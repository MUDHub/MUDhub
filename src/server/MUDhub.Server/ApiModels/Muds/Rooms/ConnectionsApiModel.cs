using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Models.Rooms;
using MUDhub.Server.ApiModels.Muds.RoomConnections;

namespace MUDhub.Server.ApiModels.Muds.Rooms
{
    public class ConnectionsApiModel
    {
        public bool South { get; set; }
        public bool North { get; set; }
        public bool West { get; set; }
        public bool East { get; set; }
        public ICollection<RoomConnectionApiModel> Portals { get; set; } = new Collection<RoomConnectionApiModel>();

        public static ConnectionsApiModel CreateFromList(IEnumerable<RoomConnection> connections, Room actualRoom)
        {
            if (connections is null)
                throw new ArgumentNullException(nameof(connections));

            if (actualRoom is null)
                throw new ArgumentNullException(nameof(actualRoom));


            ConnectionsApiModel c = new ConnectionsApiModel();
            foreach (var connection in connections)
            {
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
                else
                {
                    c.Portals.Add(RoomConnectionApiModel.ConvertFromRoomConnection(connection));
                }
            }

            return c;
        }
    }
}
