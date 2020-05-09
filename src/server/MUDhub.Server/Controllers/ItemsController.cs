using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Items;
using MUDhub.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Controllers
{
    [Route("api/muds/{mudId}")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IItemManager _itemManager;

        public ItemsController(MudDbContext context, IItemManager itemManager)
        {
            _context = context;
            _itemManager = itemManager;
        }

        [HttpGet("items")]
        [ProducesResponseType(typeof(IEnumerable<ItemApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ItemApiModel>), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllItems([FromRoute] string mudId)
        {
            return Ok(_context.Items.Where(i => i.MudGameId == mudId)
                .AsEnumerable()
                .Select(i => ItemApiModel.ConvertFromItem(i)));
        }

        [HttpPost("items")]
        [ProducesResponseType(typeof(IEnumerable<ItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ItemResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateItem([FromRoute] string mudId, [FromBody] ItemRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var createResult = await _itemManager.CreateItemAsync(HttpContext.GetUserId(), mudId,
                ItemRequest.CreateArgs(args)).ConfigureAwait(false);

            if (createResult.Success)
            {
                return Ok(new ItemResponse()
                {
                    Item = ItemApiModel.ConvertFromItem(createResult.Item!)
                });
            }
            return BadRequest(new ItemResponse()
            {
                Succeeded = false,
                Errormessage = createResult.Errormessage,
                DisplayMessage = createResult.DisplayMessage
            });
        }

        [HttpDelete("items/{itemsId}")]
        [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeleteItem([FromRoute] string mudId, [FromRoute] string itemsId)
        {
            var result = await _itemManager.DeleteItemAsync(HttpContext.GetUserId(), itemsId)
                .ConfigureAwait(false);

            if (result.Success)
            {
                return Ok(new ItemResponse());
            }

            return BadRequest(new ItemResponse()
            {
                Succeeded = false,
                Errormessage = result.Errormessage,
                DisplayMessage = result.DisplayMessage
            });
        }

        [HttpPut("items/{itemsId}")]
        [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ItemResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItem([FromRoute] string mudId, [FromRoute] string itemsId, [FromBody] ItemRequest args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var updateResult = await _itemManager.UpdateItemAsync(HttpContext.GetUserId(), itemsId,
                ItemRequest.CreateArgs(args)).ConfigureAwait(false);

            if (updateResult.Success)
            {
                return Ok(new ItemResponse()
                {
                    Item = ItemApiModel.ConvertFromItem(updateResult.Item!)
                });
            }

            return BadRequest(new ItemResponse()
            {
                Succeeded = false,
                Errormessage = updateResult.Errormessage,
                DisplayMessage = updateResult.DisplayMessage
            });
        }
    }
}