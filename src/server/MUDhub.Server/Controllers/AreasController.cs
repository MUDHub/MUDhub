using Microsoft.AspNetCore.Http;
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
    [Route("api/areas")]
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

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<AreaApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<AreaApiModel>), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllAreas([FromQuery] string? mudId = null)
        {
            return Ok(_context.Areas.Where(g => mudId == null || g.GameId == mudId)
                .AsEnumerable()
                .Select(g => AreaApiModel.ConvertFromArea(g)));
        }

        [HttpGet("{areaId}")]
        [ProducesResponseType(typeof(AreaApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AreaApiModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetArea([FromRoute] string areaId)
        {
            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                return BadRequest();
            }
            return Ok(AreaApiModel.ConvertFromArea(area));
        }

        [HttpDelete("{areaId}")]
        [ProducesResponseType(typeof(AreaDeleteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AreaDeleteResponse), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeleteArea([FromRoute] string areaId)
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
                Errormessage = result.Errormessage,
                DisplayMessage = result.DisplayMessage
            });
        }

        [HttpPost()]
        [ProducesResponseType(typeof(CreateAreaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateAreaResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateArea([FromBody] CreateAreaRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var createResult = await _areaManager.CreateAreaAsync(HttpContext.GetUserId(), args.MudId,
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
                Errormessage = createResult.Errormessage,
                DisplayMessage = createResult.DisplayMessage
            });
        }

        [HttpPut("{areaId}")]
        [ProducesResponseType(typeof(UpdateAreaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateAreaResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateArea([FromRoute] string areaId, [FromBody] UpdateAreaRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var updateResult = await _areaManager.UpdateAreaAsync(HttpContext.GetUserId(), areaId,
                UpdateAreaRequest.ConvertUpdatesArgs(args)).ConfigureAwait(false);

            if (updateResult.Success)
            {
                return Ok(new UpdateAreaResponse()
                {
                    Area = AreaApiModel.ConvertFromArea(updateResult.Area!)
                });
            }
            return BadRequest(new UpdateAreaResponse()
            {
                Succeeded = false,
                Errormessage = updateResult.Errormessage,
                DisplayMessage = updateResult.DisplayMessage
            });
        }
    }
}
