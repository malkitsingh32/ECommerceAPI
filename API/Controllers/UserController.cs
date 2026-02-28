using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO.Request;
using Application.Handler.User.Command.AddUser;
using Mapster;
using Application.Handler.User.Queries.GetUser;
using DTO.Response;
using Application.Handler.User.Queries.Login;

namespace API.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        #region Commands
        [HttpPost]
        [AllowAnonymous]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] AddUserDto addUserDto)
        {
            var command = addUserDto.Adapt<AddUserCommand>();
           return Ok(await Mediator.Send(command));
        }

        #endregion Commands

        #region Queries
        [HttpGet]
        [Route("GetUserByUserId")]
        public async Task<IActionResult> GetUsersByUserId([FromQuery] UserByUserIdDto userByUserIdDto)
        {
            var query = userByUserIdDto.Adapt<GetUserByUserIdQuery>();
            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromQuery] LoginDto loginDto)
        {
            var query = loginDto.Adapt<LoginQuery>();
            return Ok(await Mediator.Send(query));  
        }
        #endregion Queries
    }
}
    