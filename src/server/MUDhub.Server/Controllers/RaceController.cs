using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;

namespace MUDhub.Server.Controllers
{
    [Route("api/mudgame/{mudid}/Races")]
    [ApiController]
    public class RacesController : ControllerBase
    {
        private readonly ICharacterManager _characterManager;

        public RacesController(ICharacterManager characterManager)
        {
            _characterManager = characterManager;
        }

    }
}
