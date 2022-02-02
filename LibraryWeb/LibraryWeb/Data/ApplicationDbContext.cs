using LibraryWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LibraryWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<UserBook> UserBook { get; set; }

        private readonly IOptions<SeedMigration> _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<SeedMigration> config)
            : base(options)
        {
            _configuration = config;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = _configuration.Value.AdminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin",                    
                    ConcurrencyStamp = _configuration.Value.AdminRoleId
                });

            builder.Entity<IdentityUser>().HasData(
                new IdentityUser 
                {
                    Id = _configuration.Value.AdminUserId,
                    UserName = _configuration.Value.AdminUserName,
                    NormalizedUserName = _configuration.Value.AdminUserName.ToUpper(),
                    Email = _configuration.Value.AdminEmail,
                    NormalizedEmail = _configuration.Value.AdminEmail.ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = _configuration.Value.AdminPasswordHash,
                    SecurityStamp = _configuration.Value.AdminSecurityStamp,
                    ConcurrencyStamp = _configuration.Value.AdminConcurrencyStamp,
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0                   
                });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {                    
                    RoleId = _configuration.Value.AdminRoleId,
                    UserId = _configuration.Value.AdminUserId
                });            
        }
    }
}
