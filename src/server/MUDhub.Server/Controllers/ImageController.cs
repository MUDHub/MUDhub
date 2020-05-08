using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MUDhub.Core.Configurations;
using MUDhub.Server.ApiModels.Images;

namespace MUDhub.Server.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ServerConfiguration _options;
        private readonly ILogger<ImageController>? _logger;

        public ImageController(IOptions<ServerConfiguration> options, ILogger<ImageController>? logger = null)
        {
            _options = options.Value;
            _logger = logger;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ImageUploadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ImageUploadResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImageUploadAsync(IFormFile file)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));

            var imagekey = Guid.NewGuid().ToString();
            var filetype = file.FileName.Substring(file.FileName.IndexOf('.', StringComparison.InvariantCultureIgnoreCase));
            imagekey += filetype;
            var imagepath = Path.Combine(_options.ImageResourcePath, imagekey);
            try
            {
                using var imagestream = System.IO.File.Create(imagepath);
                await file.CopyToAsync(imagestream)
                    .ConfigureAwait(false);
                _logger?.LogInformation("Uploaded file {0} {1}", imagekey, imagepath);
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Can't save the image on the pyhsical drive.");
                return BadRequest(new ImageUploadResponse
                {
                    Errormessage = "Can't save the image on the server."
                });
            }
            return Ok(new ImageUploadResponse
            {
                ImageUrl = imagekey,
            });
        }


    }
}
