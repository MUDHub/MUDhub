using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Characters;
using MUDhub.Server.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/races")]
    [ApiController]
    public class RacesController : ControllerBase
    {
        private readonly ICharacterManager _characterManager;
        private readonly MudDbContext _context;

        public RacesController(ICharacterManager characterManager, MudDbContext context)
        {
            _characterManager = characterManager;
            _context = context;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RaceApiModel>), StatusCodes.Status202Accepted)]
        public IActionResult GetAllRaces([FromQuery] string? mudid = null)
           => Ok(_context.Races
                           .Where(c => mudid == null || c.GameId == mudid)
                           .AsEnumerable()
                           .Select(c => RaceApiModel.ConvertFromCharacterRace(c)));

        [HttpPost]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRaceAsync([FromBody]RaceCreateRequest request)
        {
            if (request is null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            var result = await _characterManager
                .CreateRaceAsync(HttpContext.GetUserId(), request.MudId, RaceCreateRequest.ConvertToCharacterRaceArgs(request))
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new RaceCreateResponse
                {
                    Race = RaceApiModel.ConvertFromCharacterRace(result.Race!)
                });
            }
            else
            {
                return BadRequest(new RaceCreateResponse
                {
                    Succeeded = false,
                    Errormessage = result.Errormessage
                });
            }
        }

        [HttpPut("{raceId}")]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRaceAsync([FromRoute]string raceId,  [FromBody]RaceCreateRequest request)
        {
            var result = await _characterManager
                .UpdateRaceAsync(HttpContext.GetUserId(), raceId, RaceCreateRequest.ConvertToCharacterRaceArgs(request))
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new RaceCreateResponse
                {
                    Race = RaceApiModel.ConvertFromCharacterRace(result.Race!)
                });
            }
            else
            {
                return BadRequest(new RaceCreateResponse
                {
                    Succeeded = false,
                    Errormessage = result.Errormessage
                });
            }
        }

        [HttpDelete("{raceId}")]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveRaceAsync([FromRoute]string raceId)
        {
            var result = await _characterManager
                .RemoveRaceAsync(HttpContext.GetUserId(), raceId)
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new RaceCreateResponse
                {
                    Race = RaceApiModel.ConvertFromCharacterRace(result.Race!)
                });
            }
            else
            {
                return BadRequest(new RaceCreateResponse
                {
                    Succeeded = false,
                    Errormessage = result.Errormessage
                });
            }
        }
    }
}
