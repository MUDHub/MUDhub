using System;
using MUDhub.Core.Models;

namespace MUDhub.Server.ApiModels.Muds.Areas
{
    public class AreaApiModel
    {
        public string AreaId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MudId { get; set; } = string.Empty;
        public MudApiModel Mud { get; set; } = new MudApiModel();


        public static AreaApiModel ConvertFromArea(Area area)
        {
            if (area is null)
            {
                throw new ArgumentNullException(nameof(area));
            }
            return new AreaApiModel()
            {
                Description = area.Description,
                Name = area.Name,
                AreaId = area.Id,
                MudId = area.GameId,
                Mud = MudApiModel.ConvertFromMudGame(area.Game)
            };
        }
    }
}
