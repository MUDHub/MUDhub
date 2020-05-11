using MUDhub.Core.Models.Inventories;
using System;

namespace MUDhub.Server.ApiModels.Items
{
    public class ItemApiModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Weight { get; set; }
        public string ImageKey { get; set; } = string.Empty;

        public static ItemApiModel ConvertFromItem(Item item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            return new ItemApiModel()
            {
                Description = item.Description,
                ImageKey = item.ImageKey,
                Name = item.Name,
                Weight = item.Weight,
                Id = item.Id
            };
        }
    }
}
