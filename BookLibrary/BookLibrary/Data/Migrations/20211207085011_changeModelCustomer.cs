using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrary.Data.Migrations
{
    public partial class changeModelCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_Customers_CustomerId",
                table: "UserBook");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_UserBook_CustomerId",
                table: "UserBook");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "UserBook",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Book",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBook_CustomerId",
                table: "UserBook",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBook_Customers_CustomerId",
                table: "UserBook",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
