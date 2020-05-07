using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Areas;
using MUDhub.Server.ApiModels.Muds.Areas;
using MUDhub.Server.ApiModels.Muds.RoomConnections;
using MUDhub.Server.ApiModels.Muds.Rooms;
using MUDhub.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost("areas")]
        public async Task<IActionResult> CreateArea([FromRoute] string mudId, [FromBody] CreateAreaRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var createResult = await _areaManager.CreateAreaAsync(HttpContext.GetUserId(), mudId,
                CreateAreaRequest.ConvertFromRequest(args)).ConfigureAwait(false);

            if (createResult.Success)
            {
                return Ok(new CreateAreaResponse()
                {
                    Area = AreaApiModel.ConvertFromArea(createResult.Area!)
                });
            }
            return BadRequest(new CreateAreaResponse()
            {
                Succeeded = false,
                Errormessage = "Cannot create the area."
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

        [HttpPost("areas/{areaId}/rooms")]
        public async Task<IActionResult> CreateRoom([FromRoute] string areaId, [FromBody] CreateRoomRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var createResult = await _areaManager.CreateRoomAsync(HttpContext.GetUserId(), areaId,
                CreateRoomRequest.ConvertFromRequest(args)).ConfigureAwait(false);

            if (createResult.Success)
            {
                return Ok(new CreateRoomResponse()
                {
                    Room = RoomApiModel.ConvertFromRoom(createResult.Room!)
                });
            }
            return BadRequest(new CreateAreaResponse()
            {
                Succeeded = false,
                Errormessage = "Cannot create the room."
            });
        }


        [HttpGet("areas/{areaId}/connections")]
        public async Task<IActionResult> GetAllConnections([FromRoute] string mudId, [FromRoute] string areaId, [FromQuery] string? roomId = null)
        {
            if (roomId is null)
            {
                return Ok(_context.RoomConnections.Where(r => r.GameId == mudId
                                                              && (r.Room1.Area.Id == areaId || r.Room2.Area.Id == areaId))
                    .AsEnumerable()
                    .Select(r => RoomConnectionApiModel.ConvertFromRoomConnection(r)));
            }
            var result = await _context.Rooms.FindAsync(roomId).ConfigureAwait(false);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result.Connections.Select(c => RoomConnectionApiModel.ConvertFromRoomConnection(c)));
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

        [HttpPost("areas/{areaId}/connections")]
        public async Task<IActionResult> CreateConnection([FromRoute] string areaId, [FromBody] CreateConnectionRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            if (args.LockType != LockType.NoLock
               && string.IsNullOrWhiteSpace(args.LockDescription)
               && string.IsNullOrWhiteSpace(args.LockAssociatedId))
            {
                return BadRequest(new CreateConnectionResponse()
                {
                    Succeeded = false,
                    Errormessage = "LockType is not full implemented."
                });
            }

            var createResult = await _areaManager.CreateConnectionAsync(HttpContext.GetUserId(), args.RoomId1, args.RoomId2,
                CreateConnectionRequest.ConvertFromRequest(args)).ConfigureAwait(false);

            if (createResult.Success)
            {
                return Ok(new CreateConnectionResponse()
                {
                    Connection = RoomConnectionApiModel.ConvertFromRoomConnection(createResult.RoomConnection!)
                });
            }
            return BadRequest(new CreateConnectionResponse()
            {
                Succeeded = false,
                Errormessage = "Cannot create the connection."
            });
        }
    }
}
