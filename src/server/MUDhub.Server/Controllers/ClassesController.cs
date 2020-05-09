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
    [Route("api/mudgame/{mudid}/classes")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ICharacterManager _characterManager;
        private readonly MudDbContext _context;

        public ClassesController(ICharacterManager characterManager, MudDbContext context)
        {
            _characterManager = characterManager;
            _context = context;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClassApiModel>), StatusCodes.Status202Accepted)]
        public IActionResult GetAllClasses([FromRoute] string mudid)
            => Ok(_context.Classes
                            .Where(c => c.GameId == mudid)
                            .AsEnumerable()
                            .Select(c => ClassApiModel.ConvertFromCharacterClass(c)));



        [HttpPost]
        [ProducesResponseType(typeof(ClassCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClassCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClassAsync(string mudid, ClassCreateRequest request)
        {
            var result = await _characterManager
                .CreateClassAsync(HttpContext.GetUserId(), mudid, ClassCreateRequest.ConvertToCharacterClassArgs(request))
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new ClassCreateResponse
                {
                    Class = ClassApiModel.ConvertFromCharacterClass(result.Class!)
                });
            }
            else
            {
                return BadRequest(new ClassCreateResponse
                {
                    Succeeded = false,
                    Errormessage = result.Errormessage
                });
            }
        }

        [HttpPut("{classid}")]
        [ProducesResponseType(typeof(ClassCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClassCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateClassAsync(string mudid, string classid, ClassCreateRequest request)
        {
            var result = await _characterManager
                .UpdateClassAsync(HttpContext.GetUserId(), classid, ClassCreateRequest.ConvertToCharacterClassArgs(request))
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new ClassCreateResponse
                {
                    Class = ClassApiModel.ConvertFromCharacterClass(result.Class!)
                });
            }
            else
            {
                return BadRequest(new ClassCreateResponse
                {
                    Succeeded = false,
                    Errormessage = result.Errormessage
                });
            }
        }

        [HttpDelete("{classid}")]
        [ProducesResponseType(typeof(ClassCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClassCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveClassAsync(string mudid, string classid)
        {
            var result = await _characterManager
                .RemoveClassAsync(HttpContext.GetUserId(), classid)
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new ClassCreateResponse
                {
                    Class = ClassApiModel.ConvertFromCharacterClass(result.Class!)
                });
            }
            else
            {
                return BadRequest(new ClassCreateResponse
                {
                    Succeeded = false,
                    Errormessage = result.Errormessage
                });
            }
        }

    }
}
