using MUDhub.Server.ApiModels.Muds.RoomConnections;

namespace MUDhub.Server.ApiModels.Areas
{
    public class CreateConnectionResponse : BaseResponse
    {
        public RoomConnectionApiModel Connection { get; set; }
    }
}
