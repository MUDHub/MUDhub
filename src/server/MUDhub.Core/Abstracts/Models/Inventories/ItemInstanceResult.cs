using MUDhub.Core.Models.Inventories;

namespace MUDhub.Core.Abstracts.Models.Inventories
{
    public class ItemInstanceResult : BaseResult
    {
        public ItemInstance? ItemInstance { get; set; }
        public Inventory? Inventory { get; set; }
    }
}
