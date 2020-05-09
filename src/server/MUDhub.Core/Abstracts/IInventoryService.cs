﻿using MUDhub.Core.Abstracts.Models.Inventories;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IInventoryService
    {
        Task<ItemInstanceResult> CreateItemInstance(string userId, string inventoryId);
        Task<ItemInstanceResult> RemoveItemInstance(string userId, string itemInstanceId);
        Task<ItemInstanceResult> TransferItem(string itemInstanceId, string targetInventoryId, string sourceInventoryId);
    }
}
