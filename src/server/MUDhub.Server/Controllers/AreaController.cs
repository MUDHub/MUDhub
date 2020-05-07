using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Areas;
using MUDhub.Server.ApiModels.Muds.Areas;
using MUDhub.Server.ApiModels.Muds.RoomConnections;
using MUDhub.Server.ApiModels.Muds.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUDhub.Core.Abstracts;
using MUDhub.Server.Helpers;

namespace MUDhub.Server.Controllers
{
    [Route("api/muds/{mudId}")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IAreaManager _areaManager;

        public AreaController(MudDbContext context, IAreaManager areaManager)
        {
            _context = context;
            _areaManager = areaManager;
        }

        [HttpGet("areas")]
        public ActionResult<IEnumerable<AreaApiModel>> GetAllAreas([FromRoute] string mudId)
        {
            return Ok(_context.Areas.Where(g => g.GameId == mudId)
                .AsEnumerable()
                .Select(g => AreaApiModel.ConvertFromArea(g)));
        }

        [HttpGet("areas/{areaId}")]
        public async Task<ActionResult<AreaApiModel>> GetArea([FromRoute] string mudId, [FromRoute] string areaId)
        {
            var area = await _context.Areas.FindAsync(areaId)
                .ConfigureAwait(false);
            if (area is null)
            {
                return BadRequest();
            }
            if (area.GameId == mudId)
            {
                return Ok(AreaApiModel.ConvertFromArea(area));
            }
            return BadRequest();
        }

        [HttpDelete("areas/{areaId}")]
        public async Task<ActionResult<AreaDeleteResponse>> DeleteArea([FromRoute] string areaId)
        {
            var result = await _areaManager.RemoveAreaAsync(HttpContext.GetUserId(), areaId)
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new AreaDeleteResponse());
            }

            return BadRequest(new AreaDeleteResponse()
            {
                Succeeded = false,
                Errormessage = $"Area with the Id: {areaId} does not exist!"
            });
        }

        [HttpGet("areas/{areaId}/rooms")]
        public ActionResult<IEnumerable<RoomApiModel>> GetAllRooms([FromRoute] string mudId, [FromRoute] string areaId)
        {
            return Ok(_context.Rooms.Where(r => r.GameId == mudId && r.Area.Id == areaId)
                .AsEnumerable()
                .Select(r => RoomApiModel.ConvertFromRoom(r)));
        }

        [HttpGet("areas/{areaId}/rooms/{roomId}")]
        public async Task<ActionResult<RoomApiModel>> GetRoom([FromRoute] string mudId, [FromRoute] string areaId, [FromRoute] string roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId)
                .ConfigureAwait(false);
            if (room is null)
            {
                return BadRequest();
            }
            if (room.GameId == mudId && room.AreaId == areaId)
            {
                return Ok(RoomApiModel.ConvertFromRoom(room));
            }
            return BadRequest();
        }

        [HttpDelete("areas/{areaId}/rooms/{roomId}")]
        public async Task<ActionResult<RoomDeleteResponse>> DeleteRoom([FromRoute] string roomId)
        {

            var result = await _areaManager.RemoveRoomAsync(HttpContext.GetUserId(), roomId)
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new RoomDeleteResponse());
            }

            return BadRequest(new RoomDeleteResponse()
            {
                Succeeded = false,
                Errormessage = $"Room with the Id: {roomId} does not exist!"
            });
        }

        [HttpGet("areas/connections")]
        public ActionResult<IEnumerable<RoomConnectionApiModel>> GetAllConnections([FromRoute] string mudId)
        {
            return Ok(_context.RoomConnections.Where(r => r.GameId == mudId)
                .AsEnumerable()
                .Select(r => RoomConnectionApiModel.ConvertFromRoomConnection(r)));
        }
        [HttpGet("areas/{areaId}/connections")]
        public ActionResult<IEnumerable<RoomConnectionApiModel>> GetAllConnectionsInsideArea([FromRoute] string mudId, [FromRoute] string areaId)
        {
            return Ok(_context.RoomConnections.Where(r => r.GameId == mudId
                                                          && (r.Room1.Area.Id == areaId || r.Room2.Area.Id == areaId))
                .AsEnumerable()
                .Select(r => RoomConnectionApiModel.ConvertFromRoomConnection(r)));
        }

        [HttpGet("areas/connections/{connectionId}")]
        public async Task<ActionResult<RoomConnectionApiModel>> GetConnection([FromRoute] string mudId, [FromRoute] string connectionId)
        {
            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                return BadRequest();
            }
            if (connection.GameId == mudId)
            {
                return Ok(RoomConnectionApiModel.ConvertFromRoomConnection(connection));
            }
            return BadRequest();
        }

        [HttpDelete("areas/connections/{connectionId}")]
        public async Task<ActionResult<ConnectionDeleteResponse>> DeleteConnection([FromRoute] string connectionId)
        {

            var result = await _areaManager.RemoveConnectionAsync(HttpContext.GetUserId(), connectionId)
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new ConnectionDeleteResponse());
            }

            return BadRequest(new ConnectionDeleteResponse()
            {
                Succeeded = false,
                Errormessage = $"Connection with the Id: {connectionId} does not exist!"
            });
        }
    }
}
