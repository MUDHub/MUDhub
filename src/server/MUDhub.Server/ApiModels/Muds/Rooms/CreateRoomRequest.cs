using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Query;
using MUDhub.Core.Abstracts.Models.Rooms;

namespace MUDhub.Server.ApiModels.Muds.Rooms
{
    public class CreateRoomRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
       
        public string Description { get; set; } = string.Empty;
        public string EnterMessage { get; set; } = string.Empty;

        public string ImageKey { get; set; } = string.Empty;
        [Required]
        public int X { get; set; } = 0;
        [Required]
        public int Y { get; set; } = 0;
        [Required]
        public string AreaId { get; set; }
        public bool IsDefaultRoom { get; set; } = false;

        public static RoomArgs ConvertFromRequest(CreateRoomRequest request)
        {
            if (request is null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            return new RoomArgs()
            {
                Name = request.Name,
                IsDefaultRoom = request.IsDefaultRoom,
                EnterMessage = request.EnterMessage,
                ImageKey = request.ImageKey,
                X = request.X,
                Y = request.Y,
                Description = request.Description
            };
        }
    }
}
