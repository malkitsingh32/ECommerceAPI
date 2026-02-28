using Application.Abstraction.Services;
using Application.Handler.User.Dtos;
using Common.Common;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handler.User.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, CommonResultResponseDto<UserDto>>
    {
        private readonly IUserService _userService;

        public LoginQueryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public Task<CommonResultResponseDto<UserDto>> Handle(LoginQuery loginQuery, CancellationToken cancellationToken)
        {
            return _userService.Login(loginQuery.Adapt<LoginDto>());
        }
    }
}
