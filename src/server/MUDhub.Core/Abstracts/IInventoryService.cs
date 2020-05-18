using MUDhub.Core.Abstracts.Models.Inventories;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IInventoryService
    {
        Task<ItemInstanceResult> CreateItemInstance(string userId, string inventoryId, string itemId);
        Task<ItemInstanceResult> RemoveItemInstance(string userId, string itemInstanceId);
        Task<ItemInstanceResult> TransferItemAsync(string itemInstanceId, string targetInventoryId, string sourceInventoryId);
    }
}
