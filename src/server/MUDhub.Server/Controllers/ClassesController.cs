using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;

namespace MUDhub.Server.Controllers
{
    [Route("api/mudgame/{mudid}/classes")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ICharacterManager _characterManager;

        public ClassesController(ICharacterManager characterManager)
        {
            _characterManager = characterManager;
        }


        [HttpGet]
        [ProducesResponseType(typeof(RaceApiModel))]
        public IActionResult GetAllClasses()
        {

        }

    }
}
