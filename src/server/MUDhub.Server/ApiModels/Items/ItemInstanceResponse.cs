using MUDhub.Core.Models.Inventories;

namespace MUDhub.Server.ApiModels.Items
{
    public class ItemInstanceResponse : BaseResponse
    {
        public ItemInstanceApiModel? ItemInstance { get; set; }
    }
}
