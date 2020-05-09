using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Inventories;
using System;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly MudDbContext _context;
        private readonly ILogger? _logger;

        public InventoryService(MudDbContext context, ILogger<AreaManager>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public Task<ItemInstanceResult> CreateItemInstance(string userId, string inventoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ItemInstanceResult> RemoveItemInstance(string userId, string itemInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<ItemInstanceResult> TransferItem(string itemInstanceId, string targetInventoryId, string sourceInventoryId)
        {
            throw new NotImplementedException();
        }
    }
}
