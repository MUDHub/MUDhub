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

        public static MudCreationArgs ConvertCreationArgs(MudEditRequest mudEdit, string ownerId)
        {
            return new MudCreationArgs
            {
                AutoRestart = mudEdit.AutoRestart,
                Description = mudEdit.Description,
                ImageKey = mudEdit.Imagekey,
                IsPublic = mudEdit.IsPublic,
                OwnerId = ownerId
            };
        }

        public static MudUpdateArgs ConvertUpdatesArgs(MudEditRequest mudEdit, string ownerId)
        {
            return new MudUpdateArgs
            {
                AutoRestart = mudEdit.AutoRestart,
                Description = mudEdit.Description,
                ImageKey = mudEdit.Imagekey,
                IsPublic = mudEdit.IsPublic,
                OwnerId = ownerId
            };
        }
    }
}
