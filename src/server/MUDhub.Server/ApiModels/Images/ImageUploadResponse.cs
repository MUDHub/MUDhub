using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.ApiModels.Images
{
    public class ImageUploadResponse : BaseResponse
    {
        public string ImageUrl { get; set; } = string.Empty;

    }
}
