using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Areas;
using MUDhub.Server.ApiModels.Muds.Areas;
using MUDhub.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/muds/{mudId}")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IAreaManager _areaManager;

        public AreasController(MudDbContext context, IAreaManager areaManager)
        {
            _context = context;
            _areaManager = areaManager;
        }

        [HttpGet("areas")]
        public ActionResult<IEnumerable<AreaApiModel>> GetAllAreas([FromRoute] string mudId)
        {
            return Ok(_context.Areas.Where(g => g.GameId == mudId)
                .AsEnumerable()
                .Select(g => AreaApiModel.ConvertFromArea(g)));
        }

        [HttpGet("areas/{areaId}")]
        public async Task<ActionResult<AreaApiModel>> GetArea([FromRoute] string mudId, [FromRoute] string areaId)
        {
            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                return BadRequest();
            }
            if (area.GameId == mudId)
            {
                return Ok(AreaApiModel.ConvertFromArea(area));
            }
            return BadRequest();
        }

        [HttpDelete("areas/{areaId}")]
        public async Task<ActionResult<AreaDeleteResponse>> DeleteArea([FromRoute] string areaId)
        {
            var result = await _areaManager.RemoveAreaAsync(HttpContext.GetUserId(), areaId)
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new AreaDeleteResponse());
            }

            return BadRequest(new AreaDeleteResponse()
            {
                Succeeded = false,
                Errormessage = $"Area with the Id: {areaId} does not exist!"
            });
        }

        [HttpPost("areas")]
        public async Task<IActionResult> CreateArea([FromRoute] string mudId, [FromBody] CreateAreaRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var createResult = await _areaManager.CreateAreaAsync(HttpContext.GetUserId(), mudId,
                CreateAreaRequest.ConvertFromRequest(args)).ConfigureAwait(false);

            if (createResult.Success)
            {
                return Ok(new CreateAreaResponse()
                {
                    Area = AreaApiModel.ConvertFromArea(createResult.Area!)
                });
            }
            return BadRequest(new CreateAreaResponse()
            {
                Succeeded = false,
                Errormessage = "Cannot create the area."
            });
        }
    }
}
