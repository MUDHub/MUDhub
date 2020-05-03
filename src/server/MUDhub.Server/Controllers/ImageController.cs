using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Server.ApiModels.Images;

namespace MUDhub.Server.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        public ImageController()
        {

        }


        [HttpPost]
        public ImageUploadResponse ImageUpload(IFormFile image)
        {


            return new ImageUploadResponse
            {
                ImageUrl = ""
            };
        }


    }
}
