using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyWebNovel.Application.Exceptions;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Shared.TimeProviders;
using MyWebNovel.Domain.Entities.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWebNovel.Application.Services
{
    public class JwtTokenService(IConfiguration configuration, IUnitOfWork unitOfWork, IDateTimeProvider timeProvider) : IJwtTokenService
    {
        public AccessToken GenerateAccessToken(Guid userId, string username, int roleId)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Role, roleId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var createdAt = timeProvider.UtcNow;
            var expiration = createdAt.AddMinutes(15);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration.UtcDateTime,
                signingCredentials: creds);

            return AccessToken.Create(new JwtSecurityTokenHandler().WriteToken(token), expiration.UtcDateTime, createdAt);
        }

        public async Task<AccessToken> GenerateAccessToken(Guid userId)
        {
            var account = await unitOfWork.Accounts.GetByIdAsync(userId);
            return account == null
                ? throw new NotFoundException(nameof(Account), nameof(Account.Id), userId)
                : GenerateAccessToken(account.Id, account.Username, account.RoleId);
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId)
        {
            var token = Guid.NewGuid().ToString(); // Use a secure random generator
            var createdAt = timeProvider.UtcNow;
            var expiration = createdAt.AddDays(7); // Long-lived token (e.g., 7 days)

            var refreshToken = RefreshToken.Create(userId, token, expiration, createdAt);

            await unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await unitOfWork.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token)
        {
            var refreshToken = await unitOfWork.RefreshTokens.GetByTokenAsync(token);
            if (refreshToken == null || refreshToken.IsExpired() || refreshToken.IsRevoked())
                return null;

            return refreshToken;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await unitOfWork.RefreshTokens.GetByTokenAsync(token);
            if (refreshToken == null) return false;

            refreshToken.Revoke();
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
