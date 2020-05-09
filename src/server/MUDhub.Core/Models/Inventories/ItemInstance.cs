using System;

namespace MUDhub.Core.Models.Inventories
{
    public class ItemInstance
    {
        public ItemInstance()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ItemInstance(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string ItemId { get; set; } = string.Empty;
        public virtual Item Item { get; set; }
        public string InventoryId { get; set; } = string.Empty;
        public virtual Inventory Inventory { get; set; }
    }
}
