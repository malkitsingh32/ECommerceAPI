using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Handler.User.Dtos;
using Common.Common;
using Domain.Entities;
using Helper;
using Helper.Settings;
using Mapster;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Implementation.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SymmetricSecurityKey _signingKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly Application.Abstraction.Services.IPasswordHasher _passwordHasher;
        private readonly ITokenCache _tokenCache;

        public UserService(IUserRepository userRepository, IOptions<AppSettings> appSettings, Application.Abstraction.Services.IPasswordHasher passwordHasher, ITokenCache tokenCache = null)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _passwordHasher = passwordHasher;
            _tokenCache = tokenCache;
            _tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_appSettings.Secret ?? string.Empty);
            _signingKey = new SymmetricSecurityKey(keyBytes);
            _signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        }
        public async Task<CommonResultResponseDto<int>> AddUser(Users users, string password)
        {
            if (string.IsNullOrWhiteSpace(users.Email))
            {
                throw new Exception("Email is required");
            }
            _passwordHasher.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var userId = await _userRepository.InsertUser(users, passwordHash, passwordSalt);
            return CommonResultResponseDto<int>.Success(new string[] { ActionStatusHelper.Created}, userId);
        }

        public async Task<CommonResultResponseDto<UserDto>> GetUserByUserId(int userId)
        {
            var user = await _userRepository.GetUserByUserId(userId);
            return CommonResultResponseDto<UserDto>.Success(new string[] {ActionStatusHelper.Success }, user.Adapt<UserDto>());
        }

        public async Task<CommonResultResponseDto<UserDto>> Login(LoginDto loginDto)
        {
          Users user = await _userRepository.GetUserByEmail(loginDto.Email); 
          if (!_passwordHasher.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                CommonResultResponseDto<UserDto>.Failure(new string[] { ActionStatusHelper.WrongPassword }, null, false);
            }
           var authorizedUser = await CreateJWTToken(user);
            return CommonResultResponseDto<UserDto>.Success(new string[] { ActionStatusHelper.Success }, authorizedUser);
        }

        // Password hashing/verification is provided by injected IPasswordHasher
        public Task<UserDto> CreateJWTToken(Users user)
        {
            if (user?.UserId == null)
                return Task.FromResult(GenerateToken(user));

            // Check token cache for valid cached token
            if (_tokenCache != null)
            {
                var now = DateTime.UtcNow;
                // Use a threshold to ensure cached token has reasonable TTL remaining
                var expirationThreshold = now.AddMinutes(1);
                var cachedToken = _tokenCache.GetCachedToken(user.UserId.Value, expirationThreshold);
                if (cachedToken != null)
                {
                    return Task.FromResult(cachedToken);
                }
            }

            // Generate new token and cache it
            var userDto = GenerateToken(user);
            _tokenCache?.CacheToken(user.UserId.Value, userDto);
            return Task.FromResult(userDto);
        }

        private UserDto GenerateToken(Users user)
        {
            var userId = user?.UserId?.ToString() ?? string.Empty;
            var expires = DateTime.UtcNow.AddHours(12);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }),
                Expires = expires,
                SigningCredentials = _signingCredentials
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = _tokenHandler.WriteToken(token);

            var result = user.Adapt<UserDto>();
            result.Token = tokenString;
            result.TokenExpire = expires;
            return result;
        }
    }
}
