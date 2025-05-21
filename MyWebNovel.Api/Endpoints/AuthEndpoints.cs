using MyWebNovel.Application.DTOs.Auth;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Application.Security;

namespace MyWebNovel.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/auth").WithTags($"Authentication").AddEndpointFilter<PermissionFilter>();

            group.MapPost("/register", async (IAuthService service, RegisterDto input) =>
            {
                var model = await service.RegisterAsync(input);
                return TypedResults.Created($"/api/Auth/{model.Id}", model);
            });

            group.MapPost("/login", async Task<IResult> (IAuthService authService, IJwtTokenService tokenService, LoginDto input) =>
            {
                var account = await authService.AuthenticateAsync(input);
                if (account == null)
                {
                    return TypedResults.BadRequest(new { message = "Invalid username or password." });
                }

                // Generate access token
                var accessToken = tokenService.GenerateAccessToken(account.Id, account.Username, account.RoleId);

                // Generate refresh token
                var refreshToken = await tokenService.GenerateRefreshTokenAsync(account.Id);

                return TypedResults.Ok(new LoginResponseDto(
                    AccessToken: accessToken.Value,
                    RefreshToken: refreshToken.Token,
                    ExpiresAt: accessToken.Expiration.DateTime
                ));
            });

            group.MapPost("/logout", async Task<IResult> (string refreshToken, IJwtTokenService tokenService) =>
            {
                var success = await tokenService.RevokeRefreshTokenAsync(refreshToken);
                if (!success) return TypedResults.BadRequest("Invalid refresh token.");

                return TypedResults.Ok("Logged out successfully.");
            });

            group.MapPost("/refreshToken", async Task<IResult> (IAuthService service, IJwtTokenService tokenService, RefreshTokenDto input) =>
            {
                // Validate the refresh token
                var refreshToken = await tokenService.ValidateRefreshTokenAsync(input.RefreshToken);
                if (refreshToken == null) return TypedResults.Unauthorized();

                // Generate a new access token
                var newAccessToken = await tokenService.GenerateAccessToken(refreshToken.UserId);

                // Optionally, generate a new refresh token (for increased security)
                var newRefreshToken = await tokenService.GenerateRefreshTokenAsync(refreshToken.UserId);

                // Return the new tokens
                var response = new RefreshResponseDto(
                    AccessToken: newAccessToken.Value,
                    RefreshToken: newRefreshToken.Token,
                    ExpiresAt: newAccessToken.Expiration.DateTime
                );

                return TypedResults.Ok(response);
            });

            group.MapPost("/forgotPassword", async (IAuthService service, ForgotPasswordDto input) =>
            {
                await service.ForgotPasswordAsync(input);
                return TypedResults.NoContent();
            });

            group.MapPost("/resetPassword", async (IAuthService service, ResetPasswordDto input) =>
            {
                await service.ResetPasswordAsync(input);
                return TypedResults.NoContent();
            });

            group.MapPost("/changePassword", async (IAuthService service, ChangePasswordDto input) =>
            {
                await service.ChangePasswordAsync(input);
                return TypedResults.NoContent();
            });
        }
    }
}
