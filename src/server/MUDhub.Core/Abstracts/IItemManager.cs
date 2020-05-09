using MUDhub.Core.Abstracts.Models.Inventories;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IItemManager
    {
        Task<ItemResult> CreateItemAsync(string userId, string mudId, ItemArgs args);
        Task<ItemResult> UpdateItemAsync(string userId, string itemId, ItemArgs args);
        Task<ItemResult> DeleteItemAsync(string userId, string itemId);
    }
}
