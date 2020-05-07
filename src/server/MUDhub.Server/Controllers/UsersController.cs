using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Helper;
using MUDhub.Core.Services;
using MUDhub.Server.ApiModels.Auth;
using MUDhub.Server.ApiModels.Muds;
using MUDhub.Server.ApiModels.Users;

namespace MUDhub.Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MudDbContext _context;
        private readonly IUserManager _userManager;

        public UsersController(MudDbContext dbContext, IUserManager userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }


        [HttpGet()]
        public ActionResult<IEnumerable<UserApiModel>> GetAllUsers()
            => Ok(_context.Users.AsEnumerable().Select(u => UserApiModel.CreateFromUser(u)));



        [HttpDelete("{userid}")]
        public async Task<ActionResult<UserDeleteResponse>> DeleteUser([FromRoute]string userid)
        {
            var success = await _userManager.RemoveUserAsync(userid).ConfigureAwait(false);
            if (!success)
            {
                return BadRequest(new UserDeleteResponse()
                {
                    Succeeded = false,
                    Errormessage = "The user could not be removed."
                });
            }

            return Ok();
        }


        [HttpPut("{userid}")]
        public async Task<ActionResult<UserUpdateResponse>>  UpdateUser([FromRoute]string userid, UserUpdateRequest request)
        {
            var result = await _userManager.UpdateUserAsync(userid, UserUpdateRequest.ConvertToUserArgs(request))
                .ConfigureAwait(false);

            if (result is null)
            {
                return BadRequest(new UserUpdateResponse());
            }
            else
            {
                return Ok(new UserUpdateResponse()
                {
                    User = UserApiModel.CreateFromUser(result)
                });

            }
        }


        [HttpPost("{userid}/roles")]
        public async Task<ActionResult<ChangeRoleResponse>> AddUserToRole([FromRoute] string userid, [FromQuery] string role)
        {
            var result = UserHelpers.ConvertToRole(role);
            if (result is null)
            {
                return BadRequest(new ChangeRoleResponse
                {
                    Succeeded = false,
                    Errormessage = $"Can't cast role: '{role}'"
                });
            }
            var success = await _userManager.AddRoleToUserAsync(userid, result.Value)
                                    .ConfigureAwait(false);
            if (!success)
            {
                return BadRequest(new ChangeRoleResponse()
                {
                    Succeeded = false,
                    Errormessage = "Something went wrong..." //Todo: improve response message
                });
            }

            return Ok(new ChangeRoleResponse());
        }


        [HttpDelete("{userid}/roles")]
        public async Task<ActionResult<ChangeRoleResponse>> RemoveUserFromRole([FromRoute] string userid, [FromQuery] string role)
        {
            var result = UserHelpers.ConvertToRole(role);
            if (result is null)
            {
                return BadRequest(new ChangeRoleResponse
                {
                    Succeeded = false,
                    Errormessage = $"Can't cast role: '{role}'"
                });
            }
            var success = await _userManager.RemoveRoleFromUserAsync(userid, result.Value)
                                    .ConfigureAwait(false);
            if (!success)
            {
                return BadRequest(new ChangeRoleResponse()
                {
                    Succeeded = false,
                    Errormessage = "Somehting went wrong..." //Todo: improve response message
                });
            }

            return Ok();
        }


        [HttpGet("{userid}/request")]
        public ActionResult<MudJoinsApiModel> GetMudRequests([FromRoute] string userid)
        {
            return Ok(_context.MudJoinRequests
                                .Where(mjr => mjr.UserId == userid)
                                .AsEnumerable()
                                .Select(mjr => MudJoinsApiModel.CreateFromJoin(mjr)));
        }
    }
}
