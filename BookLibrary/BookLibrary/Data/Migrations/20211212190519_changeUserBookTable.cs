using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrary.Data.Migrations
{
    public partial class changeUserBookTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_AspNetUsers_CustomerId1",
                table: "UserBook");

            migrationBuilder.DropIndex(
                name: "IX_UserBook_CustomerId1",
                table: "UserBook");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "UserBook");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "UserBook",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_AspNetUsers_CustomerId",
                table: "UserBook");

            migrationBuilder.DropIndex(
                name: "IX_UserBook_CustomerId",
                table: "UserBook");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "UserBook",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "UserBook",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBook_CustomerId1",
                table: "UserBook",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBook_AspNetUsers_CustomerId1",
                table: "UserBook",
                column: "CustomerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
