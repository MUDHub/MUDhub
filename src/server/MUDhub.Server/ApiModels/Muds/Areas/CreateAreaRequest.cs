using MUDhub.Core.Abstracts.Models.Areas;
using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Muds.Areas
{
    public class CreateAreaRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        //TODO: Soll die Description required sein?
        [Required]
        public string Description { get; set; } = string.Empty;

        public static AreaArgs ConvertFromRequest(CreateAreaRequest request)
        {
            return new AreaArgs()
            {
                Name = request.Name,
                Description = request.Description
            };
        }
    }
}
