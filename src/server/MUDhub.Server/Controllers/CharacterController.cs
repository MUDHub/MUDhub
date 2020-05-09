using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Server.ApiModels.Characters;
using MUDhub.Server.Helpers;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/mudgame/{mudid}/character")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterManager _manager;

        public CharacterController(ICharacterManager manager)
        {
            _manager = manager;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateCharacterAsync([FromRoute] string mudid, [FromBody] CharacterRequest request)
        {
            var result = await _manager.CreateCharacterAsync(HttpContext.GetUserId(), mudid, CharacterRequest.CreateArgs(request)).ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new CharacterResponse());
            }
            return Ok();
        }
    }
}
