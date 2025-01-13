using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestStoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class FifthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RequestItem");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "RequestItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "RequestItem");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RequestItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
