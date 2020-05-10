using MUDhub.Core.Models.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Items
{
    public class ItemInstanceApiModel
    {
        public string ItemInstanceId { get; set; } = string.Empty;
        public string InventoryId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public int Weight { get; set; }
        public string ImageKey { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;

        public static ItemInstanceApiModel ConvertFromItemInstance(ItemInstance itemInstance)
        {
            if (itemInstance is null)
            {
                throw new ArgumentNullException(nameof(itemInstance));
            }
            return new ItemInstanceApiModel()
            {
                ImageKey = itemInstance.Item.ImageKey,
                InventoryId = itemInstance.InventoryId,
                ItemId = itemInstance.ItemId,
                ItemInstanceId = itemInstance.Id,
                ItemName = itemInstance.Item.Name,
                Weight = itemInstance.Item.Weight
            };
        }
    }
}
