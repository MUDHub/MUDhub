using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Muds;
using MUDhub.Server.ApiModels.Muds.Areas;
using MUDhub.Server.ApiModels.Muds.RoomConnections;
using MUDhub.Server.ApiModels.Muds.Rooms;
using MUDhub.Server.Helpers;

namespace MUDhub.Server.Controllers
{
    [Route("api/muds")]
    [ApiController]
    public class MudsController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IMudManager _mudManager;
        private readonly IAreaManager _areaManager;

        public MudsController(MudDbContext context, IMudManager mudManager)
        {
            _context = context;
            _mudManager = mudManager;
        }


        [HttpGet()]
        public ActionResult<IEnumerable<MudApiModel>> GetAllMuds([FromQuery] bool fullData = false, [FromQuery] string? userid = null)
        {

            if (fullData)
            {
                //Todo: add fulld data, later with references.
            }
            else
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
            throw new NotImplementedException();
        }

        [HttpGet("{mudId}")]
        public async Task<ActionResult<MudGetResponse>> GetMud([FromRoute] string mudId)
        {
            var mud = await _context.MudGames.FindAsync(mudId)
                                                .ConfigureAwait(false);
            if (mud is null)
            {
                return BadRequest();
            }
            return Ok(MudApiModel.ConvertFromMudGame(mud));
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
                    Errormessage = "Can' create mudgame, maybe user not found" //Todo: refactor message
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
                        return BadRequest();
                    }
                case MudJoinState.Rejected:
                    {
                        var result = await _mudManager.RejectUserToJoinAsync(userid, mudId)
                                            .ConfigureAwait(false);
                        if (result)
                        {
                            return Ok();
                        }
                        return BadRequest();
                    }
                default:
                    {
                        //todo: throw exception
                        return BadRequest();
                    }
            }
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

        //TODO: Folgende Schnittstellen sind von Moris gemacht und müssen überprüft werden.

        [HttpGet("{mudId}/areas")]
        public ActionResult<IEnumerable<AreaApiModel>> GetAllAreas([FromRoute] string mudId)
        {
            return Ok(_context.Areas.Where(g => g.GameId == mudId)
                .AsEnumerable()
                .Select(g => AreaApiModel.ConvertFromArea(g)));
        }

        [HttpGet("{mudId}/areas/{areaId}")]
        public async Task<ActionResult<AreaApiModel>> GetArea([FromRoute] string mudId, [FromRoute] string areaId)
        {
            //TODO: Kann hier der Parameter "mudId" entfernt werden?
            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                return BadRequest();
            }
            return Ok(AreaApiModel.ConvertFromArea(area));
        }

        [HttpGet("{mudId}/areas/{areaId}/rooms")]
        public ActionResult<IEnumerable<RoomApiModel>> GetAllRooms([FromRoute] string mudId, [FromRoute] string areaId)
        {
            return Ok(_context.Rooms.Where(r => r.GameId == mudId && r.Area.Id == areaId)
                .AsEnumerable()
                .Select(r => RoomApiModel.ConvertFromRoom(r)));
        }

        [HttpGet("{mudId}/areas/{areaId}/rooms/{roomId}")]
        public async Task<ActionResult<RoomApiModel>> GetRoom([FromRoute] string mudId, [FromRoute] string areaId, [FromRoute] string roomId)
        {
            //TODO: Kann hier der Parameter "mudId" und "areaId" entfernt werden?
            var room = await _context.Rooms.FindAsync(roomId)
                .ConfigureAwait(false);
            if (room is null)
            {
                return BadRequest();
            }
            return Ok(RoomApiModel.ConvertFromRoom(room));
        }

        [HttpGet("{mudId}/areas/connections")]
        public ActionResult<IEnumerable<RoomConnectionApiModel>> GetAllConnections([FromRoute] string mudId)
        {
            return Ok(_context.RoomConnections.Where(r => r.GameId == mudId)
                .AsEnumerable()
                .Select(r => RoomConnectionApiModel.ConvertFromRoomConnection(r)));
        }
        [HttpGet("{mudId}/areas/{areaId}/connections")]
        public ActionResult<IEnumerable<RoomConnectionApiModel>> GetAllConnectionsInsideArea([FromRoute] string mudId, [FromRoute] string areaId)
        {
            return Ok(_context.RoomConnections.Where(r => r.GameId == mudId
                                                          && (r.Room1.Area.Id == areaId || r.Room2.Area.Id == areaId))
                .AsEnumerable()
                .Select(r => RoomConnectionApiModel.ConvertFromRoomConnection(r)));
        }

        [HttpGet("{mudId}/areas/connections/{connectionId}")]
        public async Task<ActionResult<RoomConnectionApiModel>> GetConnection([FromRoute] string mudId, [FromRoute] string connectionId)
        {
            //TODO: Kann hier der Parameter "mudId" entfernt werden?
            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                return BadRequest();
            }
            return Ok(RoomConnectionApiModel.ConvertFromRoomConnection(connection));
        }
    }
}
