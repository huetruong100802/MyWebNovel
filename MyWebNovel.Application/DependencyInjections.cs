using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Application.Security;
using MyWebNovel.Application.Services;

namespace MyWebNovel.Application
{
    public static class DependencyInjections
    {
        public static void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(builder.Configuration);

            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<INovelService, NovelService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITagService, TagService>();

            builder.Services.AddScoped<TokenCleanupService>();
            builder.Services.AddScoped(typeof(CleanupService<>));
            builder.Services.AddHostedService<CleanupBackgroundService>();

            builder.Services.AddScoped<PermissionFilter>();
            builder.Services.AddMemoryCache();
        }
    }
}
