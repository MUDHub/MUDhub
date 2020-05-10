using MUDhub.Core.Abstracts.Models.Inventories;
using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Items
{
    public class ItemInstanceRequest
    {
        [Required]
        public string ItemId { get; set; } = string.Empty;
        [Required]
        public string InventoryId { get; set; } = string.Empty;
        //[Required]
        //public string ImageKey { get; set; } = string.Empty;

        public static ItemInstanceArgs CreateArgs(ItemInstanceRequest request)
        {
            return new ItemInstanceArgs
            {
                ItemId = request.ItemId,
                InventoryId = request.InventoryId
            };
        }
    }
}
