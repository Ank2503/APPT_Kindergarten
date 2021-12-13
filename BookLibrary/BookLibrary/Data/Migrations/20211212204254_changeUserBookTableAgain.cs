using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrary.Data.Migrations
{
    public partial class changeUserBookTableAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_AspNetUsers_CustomerId",
                table: "UserBook");

            migrationBuilder.DropIndex(
                name: "IX_UserBook_CustomerId",
                table: "UserBook");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "UserBook");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserBook",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserBook");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "UserBook",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserBook_CustomerId",
                table: "UserBook",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBook_AspNetUsers_CustomerId",
                table: "UserBook",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
