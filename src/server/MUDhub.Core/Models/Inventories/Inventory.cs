using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MUDhub.Core.Models.Inventories
{
    public class Inventory
    {
        public Inventory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Inventory(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public int Capacity { get; set; } = 200;
        public int UsedCapacity { get; set; } = 0;
        public virtual ICollection<ItemInstance> ItemInstances { get; set; } = new Collection<ItemInstance>();
    }
}
