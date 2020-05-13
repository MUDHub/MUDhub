using MUDhub.Core.Abstracts.Models.Rooms;
using System;

namespace MUDhub.Server.Hubs.Models
{
    public class JoinRoomResult : SignalRBaseResult
    {
        public NavigationErrorType ErrorType { get; set; }
        public string? ActiveRoomId { get; set; }

        public static JoinRoomResult ConvertFromNavigationResult(NavigationResult result)
        {
            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return new JoinRoomResult
            {
                Success = result.Success,
                DisplayMessage = result.DisplayMessage,
                ErrorMessage = result.Errormessage,
                ErrorType = result.ErrorType,
                ActiveRoomId = result.ActiveRoom?.Id
            };
        }
    }
}
