using Microsoft.AspNetCore.Http;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Domain.Entities.Enums;
using MyWebNovel.Domain.Entities.Roles;
using System.Security.Claims;

namespace MyWebNovel.Application.Security
{
    public class PermissionFilter(IUnitOfWork unitOfWork) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var httpContext = context.HttpContext;

            // Get endpoint metadata
            var metadata = httpContext.GetEndpoint()?.Metadata.GetMetadata<EndpointPermissionMetadata>();
            if (metadata == null)
            {
                // If no metadata, assume unrestricted access
                return await next(context);
            }

            // Retrieve role (either Unauthorized or based on RoleId from JWT)
            var role = await GetUserRoleAsync(httpContext);
            if (role == null)
            {
                return Results.Forbid();
            }

            // Check if the role has the required permission
            if (!role.HasPermission(metadata.ClassPermission, metadata.PermissionLevel))
            {
                return role.Id == (int)RoleEnum.Unauthorized ? Results.Unauthorized() : Results.Forbid();
            }

            // If permission is valid, proceed
            return await next(context);
        }

        private async Task<Role?> GetUserRoleAsync(HttpContext httpContext)
        {
            // If user is not authenticated, retrieve "Unauthorized" role
            if (!httpContext.User.Identity?.IsAuthenticated ?? false)
            {
                return await unitOfWork.Roles.GetByIdAsync((int)RoleEnum.Unauthorized);
            }

            // If user is authenticated, extract RoleId from JWT
            var roleIdClaim = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrWhiteSpace(roleIdClaim) || !int.TryParse(roleIdClaim, out var roleId))
            {
                return null; // Invalid RoleId
            }

            return await unitOfWork.Roles.GetByIdAsync(roleId);
        }
    }
}
