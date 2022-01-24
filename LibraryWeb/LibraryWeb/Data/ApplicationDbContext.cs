using LibraryWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<UserBook> UserBook { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string adminId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            string roleId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = roleId,
                    Name = "Admin",
                    NormalizedName = "Admin",                    
                    ConcurrencyStamp = roleId
                });

            builder.Entity<IdentityUser>().HasData(
                new IdentityUser 
                {
                    Id = adminId,
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "admin@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEJmvWLQh18QG3DsW21SPJa4dgTXm0a0oUF+LKTGnvriCmhoHL+p20Dh/dJ8NtYtKBQ==",
                    SecurityStamp = "MKPBRCVE2GMZDZPUFTLPHWXQWBZXCG6H",
                    ConcurrencyStamp = "9f3901dd-20a5-44f7-8021-77443d4d0e52",
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
                    RoleId = roleId,
                    UserId = adminId
                });            
        }
    }
}
