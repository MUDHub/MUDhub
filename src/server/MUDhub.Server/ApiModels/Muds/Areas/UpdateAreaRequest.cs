using MUDhub.Core.Abstracts.Models.Areas;
using System.ComponentModel.DataAnnotations;

namespace MUDhub.Server.ApiModels.Muds.Areas
{
    public class UpdateAreaRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public static UpdateAreaArgs ConvertUpdatesArgs(UpdateAreaRequest requestArea)
        {
            if (requestArea is null)
            {
                throw new System.ArgumentNullException(nameof(requestArea));
            }

            return new UpdateAreaArgs
            {
                Description = requestArea.Description,
                Name = requestArea.Name
            };
        }
    }
}
