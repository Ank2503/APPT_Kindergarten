using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrary.Data.Migrations
{
    public partial class FinalChangeUserBookTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBook",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBook_BookId",
                table: "UserBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBook_UserId",
                table: "UserBook",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBook_Book_BookId",
                table: "UserBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBook_AspNetUsers_UserId",
                table: "UserBook",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_Book_BookId",
                table: "UserBook");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_AspNetUsers_UserId",
                table: "UserBook");

            migrationBuilder.DropIndex(
                name: "IX_UserBook_BookId",
                table: "UserBook");

            migrationBuilder.DropIndex(
                name: "IX_UserBook_UserId",
                table: "UserBook");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBook",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
