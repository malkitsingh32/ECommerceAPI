using Application.Handler.User.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.User.Queries.GetUser
{
    public class GetUserByUserIdQuery : IRequest<CommonResultResponseDto<UserDto>>
    {
        public int UserId { get; set; }
    }
}
