using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Inventories;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Users;
using System.Linq;
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

        /// <summary>
        /// A new item instance is created in the inventory.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="inventoryId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<ItemInstanceResult> CreateItemInstance(string userId, string inventoryId, string itemId)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }

            var inventory = await _context.Inventories.FindAsync(inventoryId)
                .ConfigureAwait(false);
            if (inventory is null)
            {
                var message = $"No inventory with the inventoryId: '{inventoryId}' was found.";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }

            var item = await _context.Items.FindAsync(itemId)
                .ConfigureAwait(false);
            if (item is null)
            {
                var message = $"No item with the itemId: '{itemId}' was found.";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }

            var itemInstance = new ItemInstance()
            {
                Inventory = inventory,
                InventoryId = inventoryId,
                Item = item,
                ItemId = itemId
            };
            _context.ItemInstances.Add(itemInstance);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"A item instance: '{itemInstance.Id}' was created in inventory: '{inventory.Id}'");
            return new ItemInstanceResult()
            {
                ItemInstance = itemInstance,
                Inventory = inventory
            };
        }

        /// <summary>
        /// An item instance is being removed from the inventory.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemInstanceId"></param>
        /// <returns></returns>
        public async Task<ItemInstanceResult> RemoveItemInstance(string userId, string itemInstanceId)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }

            var itemInstance = await _context.ItemInstances.FindAsync(itemInstanceId)
                .ConfigureAwait(false);
            if (itemInstance is null)
            {
                var message = $"No item instance with the itemInstanceId: '{itemInstanceId}' was found.";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }

            _context.ItemInstances.Remove(itemInstance);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The item instance: '{itemInstance.Id}' has been removed from the Inventory: '{itemInstance.InventoryId}'");
            return new ItemInstanceResult()
            {
                Inventory = itemInstance.Inventory,
                ItemInstance = itemInstance
            };
        }

        /// <summary>
        /// An item instance is transferred from one inventory to another inventory.
        /// </summary>
        /// <param name="itemInstanceId"></param>
        /// <param name="targetInventoryId"></param>
        /// <param name="sourceInventoryId"></param>
        /// <returns></returns>
        public async Task<ItemInstanceResult> TransferItem(string itemInstanceId, string targetInventoryId, string sourceInventoryId)
        {
            var itemInstance = await _context.ItemInstances.FindAsync(itemInstanceId)
                   .ConfigureAwait(false);
            if (itemInstance is null)
            {
                var message = $"No item instance with the itemInstanceId: '{itemInstanceId}' was found.";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }
            var targetInventory = await _context.Inventories.FindAsync(targetInventoryId)
                  .ConfigureAwait(false);
            if (targetInventory is null)
            {
                var message = $"No inventory with the inventoryId: '{targetInventoryId}' was found.";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }
            var sourceInventory = await _context.Inventories.FindAsync(sourceInventoryId)
                  .ConfigureAwait(false);
            if (sourceInventory is null)
            {
                var message = $"No inventory with the inventoryId: '{sourceInventoryId}' was found.";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }

            if (sourceInventory.ItemInstances.FirstOrDefault(i => i.Id == itemInstanceId) is null)
            {
                var message = $"No matching item instance: {itemInstanceId} was found in the source inventory: {sourceInventory.Id}";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }

            var remainingCapacity = targetInventory.Capacity - targetInventory.UsedCapacity;
            if (remainingCapacity > itemInstance.Item.Weight)
            {
                var message = $"No matching item instance: {itemInstanceId} was found in the source inventory: {sourceInventory.Id}";
                _logger?.LogWarning(message);
                return new ItemInstanceResult()
                {
                    Success = false,
                    Errormessage = message
                };
            }

            sourceInventory.ItemInstances.Remove(itemInstance);
            targetInventory.ItemInstances.Add(itemInstance);

            //TODO: Muss ich an der Db noch was machen oder wird das automatisch gespeichert?

            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The item instance: {itemInstance.Id} was transferred from inventory: {sourceInventoryId} to inventory: {targetInventoryId}");
            return new ItemInstanceResult()
            {
                ItemInstance = itemInstance,
                Inventory = targetInventory
            };
        }

        /// <summary>
        /// Is the user really the owner of the MudGame?
        /// </summary>
        /// <param name="user"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        private bool IsUserOwner(User user, string gameId)
        {
            var mudGameOwner = user.MudGames.FirstOrDefault(mg => mg.Id == gameId);
            return !(mudGameOwner is null);
        }

        /// <summary>
        /// The UserId is used to determine the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
        }
    }
}
