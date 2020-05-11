using MUDhub.Core.Abstracts.Models.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Characters
{
    public class RaceCreateResponse : BaseResponse
    {
        public RaceApiModel? Race { get; set; }

    }
}
