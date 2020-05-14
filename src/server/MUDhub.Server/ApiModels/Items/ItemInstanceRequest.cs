using MUDhub.Core.Abstracts.Models.Inventories;
using MUDhub.Core.Models.Rooms;
using System;
using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Items
{
    public class ItemInstanceRequest
    {
        [Required]
        public string ItemId { get; set; } = string.Empty;
    }
}
