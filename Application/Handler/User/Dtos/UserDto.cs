namespace Application.Handler.User.Dtos
{
    public record UserDto
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpire { get; set; }
    }
}
