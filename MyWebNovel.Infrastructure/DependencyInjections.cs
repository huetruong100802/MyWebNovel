using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Application.Shared;
using MyWebNovel.Domain.Entities.Accounts.Repositories;
using MyWebNovel.Domain.Entities.Novels.Repositories;
using MyWebNovel.Domain.Entities.Roles.Repositories;
using MyWebNovel.Domain.Entities.Shared;
using MyWebNovel.Domain.Entities.Shared.TimeProviders;
using MyWebNovel.Domain.Entities.Tokens.Repositories;
using MyWebNovel.Infrastructure.Persistence;
using MyWebNovel.Infrastructure.Persistence.Repositories;
using MyWebNovel.Infrastructure.Persistence.Repositories.Accounts;
using MyWebNovel.Infrastructure.Persistence.Repositories.Novels;
using MyWebNovel.Infrastructure.Persistence.Repositories.Roles;
using MyWebNovel.Infrastructure.Persistence.Repositories.Tokens;
using MyWebNovel.Infrastructure.Settings;
using MyWebNovel.Infrastructure.Shared;

namespace MyWebNovel.Infrastructure
{
    public static class DependencyInjections
    {
        public static void AddInfrastructureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<AppDbContext>();
            builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            DateTimeProvider.Instance = builder.Services.BuildServiceProvider().GetRequiredService<IDateTimeProvider>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Repositories to Scoped
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<INovelRepository, NovelRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<INovelTagRepository, NovelTagRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Add AppSettings
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            var appSettings = builder.Configuration.Get<AppSettings>();
            if (appSettings is not null)
            {
                builder.Services.AddSingleton(appSettings);

                // Add Email Service
                builder.Services.AddScoped<IEmailService>(sp =>
                {
                    return new EmailService(appSettings.Smtp);
                });

                // Add DbContext
                //builder.Services.AddDbContext<AppDbContext>(options =>
                //    options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection).EnableSensitiveDataLogging()
                //    );
                // Add in memory database
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase
                    (
                        databaseName: "MyWebNovel"
                    )
                );
            }
        }
    }
}
