using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Characters;
using MUDhub.Server.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/mudgame/{mudid}/character")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterManager _manager;
        private readonly MudDbContext _context;

        public CharacterController(MudDbContext context, ICharacterManager manager)
        {
            _manager = manager;
            _context = context;
        }


        [HttpPost("")]
        public async Task<IActionResult> CreateCharacterAsync([FromRoute] string mudid, [FromBody] CharacterRequest request)
        {
            var result = await _manager.CreateCharacterAsync(HttpContext.GetUserId(), mudid, CharacterRequest.CreateArgs(request)).ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new CharacterResponse());
            }
            return Ok();
        }

        [HttpDelete("{characterId}")]
        [ProducesResponseType(typeof(CharacterDeleteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CharacterDeleteResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CharacterDeleteResponse>> DeleteCharacterAsync([FromRoute] string mudid, [FromRoute] string characterId)
        {
            var result = await _manager.RemoveCharacterAsync(HttpContext.GetUserId(), characterId)
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new CharacterDeleteResponse());
            }

            return BadRequest(new CharacterDeleteResponse()
            {
                Succeeded = false,
                Errormessage = result.Errormessage
            });
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(CharacterApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CharacterApiModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCharacter([FromQuery] string gameId, [FromQuery] string userId)
        {
            if (gameId is null)
            {
                if (userId is null)
                {
                    return Ok(_context.Characters
                        .AsEnumerable()
                        .Select(c => CharacterApiModel.FromCharacter(c)));
                }
                var user = await _context.Users.FindAsync(userId).ConfigureAwait(false);
                if (user is null)
                {
                    return BadRequest($"No User found with userId: {userId}");
                }
                else
                {
                    return Ok(user.Characters.Select(c => CharacterApiModel.FromCharacter(c)));
                }
            }
            var game = await _context.MudGames.FindAsync(gameId).ConfigureAwait(false);
            if (game is null)
            {
                return BadRequest($"No MudGame found with gameId: {gameId}");
            }
            else
            {
                return Ok(game.Characters.Select(c => CharacterApiModel.FromCharacter(c)));
            }
        }
    }
}
