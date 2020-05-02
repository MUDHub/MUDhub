using MUDhub.Core.Abstracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Muds
{
    public class MudEditRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;

        public string Imagekey { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = true;
        public bool AutoRestart { get; set; } = false;

        public MudCreationArgs CreateArgs(string ownerId)
        {
            return new MudCreationArgs
            {
                AutoRestart = this.AutoRestart,
                Description = Description,
                ImageKey = Imagekey,
                IsPublic = IsPublic,
                OwnerId = ownerId
            };
        }
    }
}
