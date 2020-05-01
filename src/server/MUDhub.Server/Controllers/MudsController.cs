using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Muds;

namespace MUDhub.Server.Controllers
{
    [Route("api/muds")]
    [ApiController]
    public class MudsController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IMudManager _mudManager;

        public MudsController(MudDbContext context, IMudManager mudManager)
        {
            _context = context;
            _mudManager = mudManager;
        }


        [HttpGet()]
        public Task<ActionResult<IEnumerable<MudApiModel>>> GetAllMuds([FromQuery]bool fullData = false)
        {
            if (fullData)
            {
                //Todo: add fulld data, later with references.
            }
            else
            {
                //_context.MudGames
            }
            throw new NotImplementedException();
        }

        [HttpGet("{mudId}")]
        public MudGetResponse GetMud([FromRoute]string mudId)
        {
            throw new NotImplementedException();
        } 

        [HttpPost()]
        public MudCreationResponse CreateMud([FromBody]MudCreationRequest mudCreation)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{mudId}")]
        public MudUpdateResponse UpdateMud([FromRoute]string mudId,[FromBody] MudUpdateRequest mudUpdate)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{mudId}")]
        public MudDeleteResponse DeleteMud(string mudId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{mudId}/joins")]
        public MudJoinsResponse GetMudRequests(string mudId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{mudId}/joins/{joinId}")]
        public MudJoinsResponse ChangeRequest([FromRoute]string mudId, [FromRoute]string joinId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{mudId}/requestjoin")]
        public MudJoinsResponse RequestJoin([FromRoute]string mudId, [FromQuery]string userId)
        {
            throw new NotImplementedException();
        }
    }
}
