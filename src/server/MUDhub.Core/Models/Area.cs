using System.Collections.Generic;
using System.Collections.ObjectModel;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Models
{
    public class Area
    {
        public Area()
        {
            
        }
        public Area(string areaId)
        {
            Id = areaId;
        }

        public string Id { get; }

        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ICollection<Room> Rooms { get; set; } = new Collection<Room>();
        public MudGame Game { get; set; } = new MudGame();
    }
}
