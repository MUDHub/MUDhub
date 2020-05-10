using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Areas;
using MUDhub.Server.ApiModels.Items;
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
        private readonly IInventoryService _inventoryService;

        public RoomsController(MudDbContext context, IAreaManager areaManager, IInventoryService inventoryService)
        {
            _context = context;
            _areaManager = areaManager;
            _inventoryService = inventoryService;
        }

        [HttpGet("rooms")]
        [ProducesResponseType(typeof(IEnumerable<RoomApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<RoomApiModel>), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllRooms([FromRoute] string mudId, [FromRoute] string areaId)
        {
            return Ok(_context.Rooms.Where(r => r.GameId == mudId && r.Area.Id == areaId)
                .AsEnumerable()
                .Select(r => RoomApiModel.ConvertFromRoom(r)));
        }

        [HttpGet("rooms/{roomId}")]
        [ProducesResponseType(typeof(RoomApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RoomApiModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoom([FromRoute] string mudId, [FromRoute] string areaId, [FromRoute] string roomId)
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
        [ProducesResponseType(typeof(RoomDeleteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RoomDeleteResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRoom([FromRoute] string roomId)
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
                Errormessage = result.Errormessage,
                DisplayMessage = result.DisplayMessage,
                IsDefaultRoom = result.IsDefaultRoom
            });
        }
        [HttpDelete("rooms/{roomId}/iteminstances/{iteminstancesId}")]
        [ProducesResponseType(typeof(RoomDeleteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RoomDeleteResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteItemInstances([FromRoute] string roomId, [FromRoute] string iteminstancesId)
        {
            var result = await _inventoryService.RemoveItemInstance(HttpContext.GetUserId(), iteminstancesId)
                .ConfigureAwait(false);
            if (result.Success)
            {
                return Ok(new ItemInstanceResponse());
            }

            return BadRequest(new ItemInstanceResponse()
            {
                Succeeded = false,
                Errormessage = result.Errormessage,
                DisplayMessage = result.DisplayMessage
            });
        }

        [HttpPost("rooms")]
        [ProducesResponseType(typeof(CreateRoomResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateRoomResponse), StatusCodes.Status400BadRequest)]
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
                Errormessage = createResult.Errormessage,
                DisplayMessage = createResult.DisplayMessage
            });
        }
        
        [HttpPost("rooms/{roomId}/iteminstances")]
        [ProducesResponseType(typeof(CreateRoomResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateRoomResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateItemInstance([FromRoute] string roomId, [FromBody] ItemInstanceRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var createResult = await _inventoryService.CreateItemInstance(HttpContext.GetUserId(), args.InventoryId,
                args.ItemId).ConfigureAwait(false);

            if (createResult.Success)
            {
                return Ok(new ItemInstanceResponse()
                {
                    ItemInstance = ItemInstanceApiModel.ConvertFromItemInstance(createResult.ItemInstance!)
                });
            }
            return BadRequest(new ItemInstanceResponse()
            {
                Succeeded = false,
                Errormessage = createResult.Errormessage,
                DisplayMessage = createResult.DisplayMessage
            });
        }

        [HttpPut("rooms/{roomId}")]
        [ProducesResponseType(typeof(UpdateRoomResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateRoomResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRoom([FromRoute] string roomId, [FromBody] UpdateRoomRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var updateResult = await _areaManager.UpdateRoomAsync(HttpContext.GetUserId(), roomId,
                UpdateRoomRequest.ConvertUpdatesArgs(args)).ConfigureAwait(false);

            if (updateResult.Success)
            {
                return Ok(new UpdateRoomResponse()
                {
                    Room = RoomApiModel.ConvertFromRoom(updateResult.Room!)
                });
            }
            return BadRequest(new UpdateRoomResponse()
            {
                Succeeded = false,
                Errormessage = updateResult.Errormessage,
                DisplayMessage = updateResult.DisplayMessage
            });
        }
    }
}