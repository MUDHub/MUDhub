using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Server.ApiModels.Muds.Rooms;
using System;

namespace MUDhub.Server.Hubs.Models
{
    public class EnterRoomResult : SignalRBaseResult
    {
        public NavigationErrorType ErrorType { get; set; }
        public RoomApiModel? ActiveRoom { get; set; }
        public string? ActiveAreaId { get; set; }

        public static EnterRoomResult ConvertFromNavigationResult(NavigationResult result)
        {
            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return new EnterRoomResult
            {
                Success = result.Success,
                DisplayMessage = result.DisplayMessage,
                ErrorMessage = result.Errormessage,
                ErrorType = result.ErrorType,
                ActiveRoom = RoomApiModel.ConvertFromRoom(result.ActiveRoom!,false),
                ActiveAreaId = result.ActiveRoom?.AreaId
            };
        }
    }
}
