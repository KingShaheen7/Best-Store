using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestStoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class EditRequestItems1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItem_Users_RequestUserUserId",
                table: "RequestItem");

            migrationBuilder.DropIndex(
                name: "IX_RequestItem_RequestUserUserId",
                table: "RequestItem");

            migrationBuilder.DropColumn(
                name: "RequestUserUserId",
                table: "RequestItem");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RequestItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RequestItem");

            migrationBuilder.AddColumn<int>(
                name: "RequestUserUserId",
                table: "RequestItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestItem_RequestUserUserId",
                table: "RequestItem",
                column: "RequestUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItem_Users_RequestUserUserId",
                table: "RequestItem",
                column: "RequestUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
