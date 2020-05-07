using MUDhub.Server.ApiModels.Muds.Areas;

namespace MUDhub.Server.ApiModels.Areas
{
    public class CreateAreaResponse : BaseResponse
    {
        public AreaApiModel Area { get; set; }
    }
}
