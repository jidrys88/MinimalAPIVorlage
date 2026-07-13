using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using DataModels.Dtos;
using DataModels.Entities;
using DBUmgebung.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProduktService.Interfaces;

namespace ProduktService
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _users;
        private readonly IConfiguration _configuration;

        public AuthService(IGenericRepository<User> users, IConfiguration configuration)
        {
            _users = users;
            _configuration = configuration;
        }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(ApiRequest<RegisterDto> request)
        {
            if (request.Data == null)
                return ApiResponse<AuthResponseDto>.Fail("Registrierungsdaten fehlen");

            var errors = ValidationHelper.Validate(request.Data);
            if (errors.Count > 0)
                return ApiResponse<AuthResponseDto>.Fail("Validierung fehlgeschlagen", errors);

            var existing = await _users.FindAsync(u => u.Username == request.Data.Username);
            if (existing != null)
                return ApiResponse<AuthResponseDto>.Fail("Username bereits vergeben");

            var user = new User
            {
                Username = request.Data.Username,
                PasswordHash = PasswordHasher.Hash(request.Data.Password)
            };

            await _users.AddAsync(user);
            await _users.SaveAsync();

            return ApiResponse<AuthResponseDto>.Ok(GenerateToken(user));
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(ApiRequest<LoginDto> request)
        {
            if (request.Data == null)
                return ApiResponse<AuthResponseDto>.Fail("Logindaten fehlen");

            var errors = ValidationHelper.Validate(request.Data);
            if (errors.Count > 0)
                return ApiResponse<AuthResponseDto>.Fail("Validierung fehlgeschlagen", errors);

            var user = await _users.FindAsync(u => u.Username == request.Data.Username);
            if (user == null || !PasswordHasher.Verify(request.Data.Password, user.PasswordHash))
                return ApiResponse<AuthResponseDto>.Fail("Username oder Password falsch");

            return ApiResponse<AuthResponseDto>.Ok(GenerateToken(user));
        }

        private AuthResponseDto GenerateToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key fehlt in appsettings.json");

            var expiresMinutes = int.Parse(_configuration["Jwt:ExpiresMinutes"] ?? "60");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires,
                Username = user.Username
            };
        }
    }
}
