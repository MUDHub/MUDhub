using MUDhub.Core.Models.Muds;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models.Inventories
{
    public class Item
    {
        public Item()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Item(string id)
        {
            Id = id;
        }
        public string Id { get; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Weight { get; set; }
        public string ImageKey { get; set; } = string.Empty;
        public string MudGameId { get; set; } = string.Empty;
        public virtual MudGame MudGame { get; set; }
    }
}
