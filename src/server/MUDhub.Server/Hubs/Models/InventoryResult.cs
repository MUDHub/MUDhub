using MUDhub.Server.ApiModels.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs.Models
{
    public class InventoryResult : SignalRBaseResult
    {
        public IEnumerable<ItemInstanceApiModel> Items { get; set; }
    }
}
