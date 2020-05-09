using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Services;

namespace MUDhub.Server.Controllers
{
    [Route("api/muds/{mudid}/areas/{areaid}/rooms/{roomid}")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IInventoryService _inventoryService;

        public InventoryController(MudDbContext context, IInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }
    }
}