using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrary.Data.Migrations
{
    public partial class SeedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name], [Discriminator]) VALUES (N'0173676b-9b5c-4e04-a37d-70fd6e997c73', N'admin@gmail.com', N'ADMIN@GMAIL.COM', N'admin@gmail.com', N'ADMIN@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEHtfN5pD+oSswgW1tudkHbqvPK5dehjP1p/my+QiylhQMhyC0TyxF4wFCxDRZT3yAg==', N'S4DMO36WA25ZCM4E5C24YLHXK4QPGQ3H', N'500780c0-52e2-4bda-b767-f7419015d691', NULL, 0, 0, NULL, 1, 0, NULL, N'IdentityUser')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'0f3687ed-0a26-4cfe-8988-184570a6d151', N'admin', N'ADMIN', N'26b5e0c8-2894-4bc5-8e5c-a484efdf93f3')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0173676b-9b5c-4e04-a37d-70fd6e997c73', N'0f3687ed-0a26-4cfe-8988-184570a6d151')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
