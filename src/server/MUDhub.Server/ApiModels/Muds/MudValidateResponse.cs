using MUDhub.Core.Abstracts.Models.Muds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Muds
{
    public class MudValidateResponse : BaseResponse
    {
        public IEnumerable<MudValidateErrorMessage> ValidationErrors { get; set; }
        public bool IsMudValid { get; set; } = false;
    }
}
