using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models.Characters;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels;
using MUDhub.Server.ApiModels.Characters;
using MUDhub.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/characters")]
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

        [HttpPost()]
        [ProducesResponseType(typeof(CharacterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCharacterAsync([FromBody] CharacterRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = await _manager.CreateCharacterAsync(HttpContext.GetUserId(), request.MudId, CharacterRequest.CreateArgs(request)).ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new CharacterResponse
                {
                    Character = CharacterApiModel.FromCharacter(result.Character!)
                });
            }
            return BadRequest();
        }

        [HttpDelete("{characterId}")]
        [ProducesResponseType(typeof(CharacterDeleteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CharacterDeleteResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CharacterDeleteResponse>> DeleteCharacterAsync([FromRoute] string characterId)
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

        [HttpGet("{characterId}")]
        [ProducesResponseType(typeof(CharacterApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CharacterApiModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCharacter([FromQuery] string characterId)
        {
            var character = await _context.Characters.FindAsync(characterId).ConfigureAwait(false);
            if (character is null)
            {
                return NotFound();
            }
            return Ok(CharacterApiModel.FromCharacter(character));
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<CharacterApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCharacters([FromQuery] string? mudId = null, [FromQuery] string? userId = null)
        {
            if (mudId is null && userId is null)
            {
                return Ok(_context.Characters
                   .AsEnumerable()
                   .Select(c => CharacterApiModel.FromCharacter(c)));
            }
            if (!(userId is null))
            {
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
            if(!(mudId is null))
            {
                var game = await _context.MudGames.FindAsync(mudId).ConfigureAwait(false);
                if (game is null)
                {
                    return BadRequest($"No MudGame found with gameId: {mudId}");
                }
                else
                {
                    return Ok(game.Characters.Select(c => CharacterApiModel.FromCharacter(c)));
                }
            }

            //Note: should never append
            return BadRequest();
        }
    }
}
