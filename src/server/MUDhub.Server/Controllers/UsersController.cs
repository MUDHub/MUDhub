using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Auth;

namespace MUDhub.Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MudDbContext _dbContext;

        public UsersController(MudDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet()]
        public ActionResult<IEnumerable<UserApiModel>> GetAllUsers() 
            => Ok(_dbContext.Users.AsEnumerable().Select(u => UserApiModel.CreateFromUser(u)));
    }
}
