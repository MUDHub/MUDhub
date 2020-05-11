using MUDhub.Core.Abstracts.Models.Inventories;
using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Items
{
    public class ItemRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Weight { get; set; }
        public string ImageKey { get; set; } = string.Empty;

        public static ItemArgs CreateArgs(ItemRequest request)
        {
            if (request is null)
                throw new System.ArgumentNullException(nameof(request));

            return new ItemArgs
            {
                Name = request.Name,
                Description = request.Description,
                ImageKey = request.ImageKey,
                Weight = request.Weight
            };
        }
    }
}
