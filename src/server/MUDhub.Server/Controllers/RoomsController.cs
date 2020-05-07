using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Areas;
using MUDhub.Server.ApiModels.Muds.Rooms;
using MUDhub.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/muds/{mudId}/areas/{areaId}")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IAreaManager _areaManager;

        public RoomsController(MudDbContext context, IAreaManager areaManager)
        {
            _context = context;
            _areaManager = areaManager;
        }

        [HttpGet("rooms")]
        public ActionResult<IEnumerable<RoomApiModel>> GetAllRooms([FromRoute] string mudId, [FromRoute] string areaId)
        {
            return Ok(_context.Rooms.Where(r => r.GameId == mudId && r.Area.Id == areaId)
                .AsEnumerable()
                .Select(r => RoomApiModel.ConvertFromRoom(r)));
        }

        [HttpGet("rooms/{roomId}")]
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

        [HttpDelete("rooms/{roomId}")]
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

        [HttpPost("rooms")]
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
    }
}