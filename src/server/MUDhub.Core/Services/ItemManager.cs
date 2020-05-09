using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Inventories;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class ItemManager : IItemManager
    {
        private readonly MudDbContext _context;
        private readonly ILogger? _logger;

        public ItemManager(MudDbContext context, ILogger<AreaManager>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public Task<ItemResult> CreateItem(string userId, string mudId, ItemArgs args)
        {
            throw new System.NotImplementedException();
        }

        public Task<ItemResult> UpdateItem(string userId, string itemId, ItemArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
