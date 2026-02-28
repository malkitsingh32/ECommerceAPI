using Application.Abstraction.Services;
using Application.Handler.User.Dtos;
using MediatR;
using Mapster;
using Common.Common;

namespace Application.Handler.User.Queries.GetUser
{
    public class GetUserByUserIdQueryHandler : IRequestHandler<GetUserByUserIdQuery, CommonResultResponseDto<UserDto>>
    {
        private readonly IUserService _userService;
        public GetUserByUserIdQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CommonResultResponseDto<UserDto>> Handle(GetUserByUserIdQuery getUsersByUserIdQuery, CancellationToken cancellationToken)
        {
            return await _userService.GetUserByUserId(getUsersByUserIdQuery.UserId);
        }
    }
}
