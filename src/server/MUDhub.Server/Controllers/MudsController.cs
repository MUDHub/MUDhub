using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Muds;
using MUDhub.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/muds")]
    [ApiController]
    public class MudsController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IMudManager _mudManager;
        private readonly IGameService _gameService;

        public MudsController(MudDbContext context, IMudManager mudManager, IGameService gameService)
        {
            _context = context;
            _mudManager = mudManager;
            _gameService = gameService;
        }


        [HttpGet()]
        public ActionResult<IEnumerable<MudApiModel>> GetAllMuds([FromQuery] string? userid = null)
        {
            if (userid is null)
            {
                return Ok(_context.MudGames.Include(mg => mg.Owner)
                                           .AsEnumerable()
                                           .Select(mg => MudApiModel.ConvertFromMudGame(mg)));
            }
            else
            {
                return Ok(_context.MudGames.Where(g => g.OwnerId == userid)
                                            .Include(mg => mg.Owner)
                                            .AsEnumerable()
                                            .Select(mg => MudApiModel.ConvertFromMudGame(mg)));
            }
        }

        [HttpGet("{mudId}")]
        public async Task<IActionResult> GetMud([FromRoute] string mudId)
        {
            var mud = await _context.MudGames.FindAsync(mudId)
                                                .ConfigureAwait(false);
            if (mud is null)
            {
                return BadRequest();
            }
            return Ok(MudApiModel.ConvertFromMudGame(mud));
        }

        [HttpGet("{mudId}/validate")]
        [ProducesResponseType(typeof(MudValidateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MudValidateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateMud([FromRoute] string mudId)
        {
            var result = await _mudManager.ValidateMudAsync(mudId).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest(new MudValidateResponse
                {
                    Succeeded = false,
                    DisplayMessage = "Mud wurde nicht gefunden",
                    Errormessage = result.ExecutionError
                });
            }
            return Ok(new MudValidateResponse
            {
                Succeeded = true,
                IsMudValid = result.Valid,
                ValidationErrors = result.ErrorMessages,
            });
        }

        [HttpPost()]
        public async Task<ActionResult<MudCreationResponse>> CreateMud([FromBody] MudEditRequest mudCreation)
        {
            if (mudCreation is null)
                throw new ArgumentNullException(nameof(mudCreation));

            var mud = await _mudManager.CreateMudAsync(mudCreation.Name, MudEditRequest.ConvertCreationArgs(mudCreation, HttpContext.GetUserId()))
                .ConfigureAwait(false);

            if (mud is null)
            {
                //Todo: later add usefull message.
                return BadRequest(new MudCreationResponse
                {
                    Succeeded = false,
                    Errormessage = "Can' create mudgame, maybe user not found"
                });
            }
            return Ok(new MudCreationResponse
            {
                Mud = MudApiModel.ConvertFromMudGame(mud)
            });
        }

        [HttpPut("{mudId}")]
        public async Task<ActionResult<MudUpdateResponse>> UpdateMud([FromRoute] string mudId, [FromBody] MudEditRequest mudUpdate)
        {
            var result = await _mudManager.UpdateMudAsync(mudId, MudEditRequest.ConvertUpdatesArgs(mudUpdate, HttpContext.GetUserId()))
                .ConfigureAwait(false);

            if (result is null)
            {
                return BadRequest(new MudUpdateResponse());
            }
            else
            {
                return Ok(new MudUpdateResponse());
            }
        }

        [HttpDelete("{mudId}")]
        public async Task<ActionResult<MudDeleteResponse>> DeleteMud(string mudId)
        {
            var result = await _mudManager.RemoveMudAsync(mudId)
                                .ConfigureAwait(false);
            if (!result)
                return BadRequest(new MudDeleteResponse
                {
                    Errormessage = $"Mud with the Id '{mudId}' does not exist!",
                    Succeeded = false
                });

            return Ok(new MudDeleteResponse());
        }

        [HttpGet("{mudId}/request")]
        public ActionResult<MudJoinsApiModel> GetMudRequests([FromRoute] string mudId)
        {
            return Ok(_context.MudJoinRequests
                                .Include(mjr => mjr.User)
                                .Where(mjr => mjr.MudId == mudId)
                                .AsEnumerable()
                                .Select(mjr => MudJoinsApiModel.CreateFromJoin(mjr)));
        }

        [HttpPut("{mudId}/request/{userid}")]
        public async Task<IActionResult> ChangeRequestAsync([FromRoute] string mudId, [FromRoute] string userid, [FromBody] int state)
        {
            switch ((MudJoinState)state)
            {
                case MudJoinState.Accepted:
                {
                    var result = await _mudManager.ApproveUserToJoinAsync(userid, mudId)
                                        .ConfigureAwait(false);
                    if (result)
                    {
                        return Ok();
                    }
                }
                break;
                case MudJoinState.Rejected:
                {
                    var result = await _mudManager.RejectUserToJoinAsync(userid, mudId)
                                        .ConfigureAwait(false);
                    if (result)
                    {
                        return Ok();
                    }
                }
                break;
            }
            return BadRequest();
        }

        [HttpPost("{mudId}/requestjoin")]
        public async Task<ActionResult<MudJoinsResponse>> RequestJoin([FromRoute] string mudId)
        {
            var result = await _mudManager.RequestUserForJoinAsync(HttpContext.GetUserId(), mudId)
                                           .ConfigureAwait(false);
            if (!result)
            {
                return BadRequest(new MudJoinsResponse
                {
                    Succeeded = false,
                    Errormessage = "Something went wrong" //Todo: improve repsonse message
                });
            }
            else
            {
                return Ok(new MudJoinsResponse());
            }
        }

        [HttpPost("{mudId}/start")]
        public async Task<ActionResult> StartMudAsync([FromRoute] string mudId)
        {
            var result = await _gameService.StartMudAsync(mudId, HttpContext.GetUserId())
                                            .ConfigureAwait(false);
            if (result)
                return Ok();
            else
                return BadRequest();

        }

        [HttpPost("{mudId}/stop")]
        public async Task<ActionResult> StopMudAsync([FromRoute] string mudId)
        {
            var result = await _gameService.StopMudAsync(mudId, HttpContext.GetUserId())
                                            .ConfigureAwait(false);
            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("{mudId}/edit")]
        public async Task<ActionResult> EditMudAsync([FromRoute] string mudId, [FromQuery] bool isInEdit)
        {
            var result = await _mudManager.SetEditModeAsync(mudId, HttpContext.GetUserId(), isInEdit)
                                            .ConfigureAwait(false);
            if (result)
                return Ok();
            else
                return BadRequest();

        }
    }
}
