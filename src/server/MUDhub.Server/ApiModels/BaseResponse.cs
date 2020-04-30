using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels
{
    public class BaseResponse
    {
        public int Statuscode { get; set; } = StatusCodes.Status200OK;
        public string Errormessage { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = true;
    }
}
