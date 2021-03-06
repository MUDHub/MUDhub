﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Models.Connections;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Areas;
using MUDhub.Server.ApiModels.Muds.RoomConnections;
using MUDhub.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/connections")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IAreaManager _areaManager;

        public ConnectionsController(MudDbContext context, IAreaManager areaManager)
        {
            _context = context;
            _areaManager = areaManager;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<RoomConnectionApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<RoomConnectionApiModel>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllConnections([FromQuery] string? roomId = null, [FromQuery] string? areaId = null)
        {
            if (roomId is null)
            {
                return Ok(_context.RoomConnections.Where(r => r.Room1.Area.Id == areaId || r.Room2.Area.Id == areaId)
                    .AsEnumerable()
                    .Select(r => RoomConnectionApiModel.ConvertFromRoomConnection(r)));
            }
            var result = await _context.Rooms.FindAsync(roomId).ConfigureAwait(false);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result.AllConnections.Select(c => RoomConnectionApiModel.ConvertFromRoomConnection(c)));
        }

        [HttpGet("{connectionId}")]
        [ProducesResponseType(typeof(RoomConnectionApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RoomConnectionApiModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomConnectionApiModel>> GetConnection([FromRoute] string connectionId)
        {
            var connection = await _context.RoomConnections.FindAsync(connectionId)
                .ConfigureAwait(false);
            if (connection is null)
            {
                return BadRequest();
            }
            return Ok(RoomConnectionApiModel.ConvertFromRoomConnection(connection));
        }

        [HttpDelete("{connectionId}")]
        [ProducesResponseType(typeof(ConnectionDeleteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ConnectionDeleteResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteConnection([FromRoute] string connectionId)
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
                Errormessage = result.Errormessage
            });
        }

        [HttpPost()]
        [ProducesResponseType(typeof(CreateConnectionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateConnectionResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateConnection([FromBody] CreateConnectionRequest args)
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
                Errormessage = createResult.Errormessage,
                DisplayMessage = createResult.DisplayMessage
            });
        }

        [HttpPut("{connectionId}")]
        [ProducesResponseType(typeof(UpdateConnectionRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UpdateConnectionRequest), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateConnection([FromRoute] string connectionId, [FromBody] UpdateConnectionRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var updateResult = await _areaManager.UpdateConnectionAsync(HttpContext.GetUserId(), connectionId,
                UpdateConnectionRequest.ConvertUpdatesArgs(args)).ConfigureAwait(false);

            if (updateResult.Success)
            {
                return Ok(new UpdateConnectionResponse()
                {
                    Connection = RoomConnectionApiModel.ConvertFromRoomConnection(updateResult.RoomConnection!)
                });
            }
            return BadRequest(new UpdateConnectionResponse()
            {
                Succeeded = false,
                Errormessage = $"Cannot update the connection: '{connectionId}'"
            });
        }

    }
}