using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class levelOk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelBOk",
                table: "CyPCBOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LevelOk",
                table: "CyPCBOrder",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelBOk",
                table: "CyPCBOrder");

            migrationBuilder.DropColumn(
                name: "LevelOk",
                table: "CyPCBOrder");
        }
    }
}
