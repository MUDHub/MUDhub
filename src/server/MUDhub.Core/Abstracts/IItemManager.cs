using MUDhub.Core.Abstracts.Models.Inventories;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IItemManager
    {
        Task<ItemResult> CreateItem(string userId, string mudId, ItemArgs args);
        Task<ItemResult> UpdateItem(string userId, string itemId, ItemArgs args);
    }
}
