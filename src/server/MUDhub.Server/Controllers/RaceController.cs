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
    [Route("api/muds/{mudid}/races")]
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
        public IActionResult GetAllRaces([FromRoute] string mudid)
           => Ok(_context.Races
                           .Where(c => c.GameId == mudid)
                           .AsEnumerable()
                           .Select(c => RaceApiModel.ConvertFromCharacterRace(c)));

        [HttpPost]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRaceAsync(string mudid, RaceCreateRequest request)
        {
            var result = await _characterManager
                .CreateRaceAsync(HttpContext.GetUserId(), mudid, RaceCreateRequest.ConvertToCharacterRaceArgs(request))
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

        [HttpPut("{raceid}")]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRaceAsync(string mudid, string raceid, RaceCreateRequest request)
        {
            var result = await _characterManager
                .UpdateRaceAsync(HttpContext.GetUserId(), raceid, RaceCreateRequest.ConvertToCharacterRaceArgs(request))
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

        [HttpDelete("{raceid}")]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RaceCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveRaceAsync(string mudid, string raceid)
        {
            var result = await _characterManager
                .RemoveRaceAsync(HttpContext.GetUserId(), raceid)
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
