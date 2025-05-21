using Microsoft.EntityFrameworkCore;
using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Enums;
using MyWebNovel.Domain.Entities.Novels;
using MyWebNovel.Domain.Entities.Roles;
using MyWebNovel.Domain.Entities.Tokens;
namespace MyWebNovel.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Novel> Novels => Set<Novel>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<NovelTag> Tags => Set<NovelTag>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                await SeedRolesWithPermissions(context, cancellationToken);
                await SeedTags(context, cancellationToken);
                await SeedAccounts(context, cancellationToken);
            });
        }

        private static async Task SeedAccounts(DbContext context, CancellationToken cancellationToken)
        {
            var accountsToSeed = new List<Account>
            {
                Account.Create("admin", "huetruong100802@gmail.com", "@Admin123@", (int)RoleEnum.Admin)
            };
            await context.Set<Account>().AddRangeAsync(accountsToSeed, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }


        private static async Task SeedRolesWithPermissions(DbContext context, CancellationToken cancellationToken)
        {
            // Define Roles
            var adminRole = Role.Create((int)RoleEnum.Admin, RoleEnum.Admin.ToString());
            adminRole.AddPermissions([
                new(nameof(Account), PermissionLevelEnum.FullAccess),
                new(nameof(Role), PermissionLevelEnum.FullAccess),
                new(nameof(NovelTag), PermissionLevelEnum.FullAccess),
                new(nameof(Novel), PermissionLevelEnum.View)
            ]);

            var memberRole = Role.Create((int)RoleEnum.Member, RoleEnum.Member.ToString());
            memberRole.AddPermissions([
                new(nameof(Account), PermissionLevelEnum.Modifed),
                new(nameof(Role), PermissionLevelEnum.AccessDenied),
                new(nameof(NovelTag), PermissionLevelEnum.View),
                new (nameof(Novel), PermissionLevelEnum.FullAccess),
            ]);

            var unauthorizedRole = Role.Create((int)RoleEnum.Unauthorized, RoleEnum.Unauthorized.ToString());
            unauthorizedRole.AddPermissions([
                new(nameof(Account), PermissionLevelEnum.AccessDenied),
                new(nameof(Role), PermissionLevelEnum.AccessDenied),
                new(nameof(NovelTag), PermissionLevelEnum.View),
                new(nameof(Novel), PermissionLevelEnum.View),
            ]);

            // Add Roles to Context
            var rolesToSeed = new List<Role> { adminRole, memberRole, unauthorizedRole };

            foreach (var role in rolesToSeed)
            {
                if (!await context.Set<Role>().AnyAsync(r => r.Name == role.Name, cancellationToken))
                {
                    await context.Set<Role>().AddAsync(role, cancellationToken);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task SeedTags(DbContext context, CancellationToken cancellationToken)
        {
            var TagsToSeed = new List<NovelTag>
            {
                NovelTag.Create("Fantasy", NovelTag.TagType.Genre),
                NovelTag.Create("Science Fiction", NovelTag.TagType.Genre),
                NovelTag.Create("Mystery", NovelTag.TagType.Genre),
                NovelTag.Create("Romance", NovelTag.TagType.Genre),
                NovelTag.Create("Horror", NovelTag.TagType.Genre),
                NovelTag.Create("Biography", NovelTag.TagType.Genre),
                NovelTag.Create("Non-Fiction", NovelTag.TagType.Genre),
                NovelTag.Create("Fiction", NovelTag.TagType.Genre)
            };

            foreach (var Tag in TagsToSeed)
            {
                if (!await context.Set<NovelTag>().AnyAsync(g => g.Name == Tag.Name, cancellationToken))
                {
                    await context.Set<NovelTag>().AddAsync(Tag, cancellationToken);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }

    }
}
