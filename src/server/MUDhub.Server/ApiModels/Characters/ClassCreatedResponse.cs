using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Characters
{
    public class ClassCreateResponse :BaseResponse
    {
        public ClassApiModel? Class { get; set; }

    }
}
