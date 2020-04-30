using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels
{
    public class BaseResponse
    {
        public int Statuscode { get; set; } = 200;
        public string Errormessage { get; set; } = string.Empty;
        public bool Succeeded { get; set; }
    }
}
