using Application.Abstraction.Services;
using Common.Common;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Handler.User.Command.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, CommonResultResponseDto<int>>
    {
        private readonly IUserService _userService;

        public AddUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CommonResultResponseDto<int>> Handle(AddUserCommand addUserCommand, CancellationToken cancellationToken)
        {
           return await _userService.AddUser(addUserCommand.Adapt<Users>(), addUserCommand.Password); 
        }
    }
}
