using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrary.Data.Migrations
{
    public partial class FinalChangeUserBookTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_Book_BookId",
                table: "UserBook");

            migrationBuilder.DropIndex(
                name: "IX_UserBook_BookId",
                table: "UserBook");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserBook_BookId",
                table: "UserBook",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBook_Book_BookId",
                table: "UserBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
