﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MUDhub.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("")]
        public IEnumerable<string> GetStrings()
        {
            yield return "hallo";
            yield return "hallo";
            yield return "hallo";
            yield return "hallo";
        }
    }
}
