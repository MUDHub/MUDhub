using MUDhub.Server.ApiModels.Muds.Rooms;

namespace MUDhub.Server.ApiModels.Areas
{
    public class CreateRoomResponse : BaseResponse
    {
        public RoomApiModel Room { get; set; }
    }
}
