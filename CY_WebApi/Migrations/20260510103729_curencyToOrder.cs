using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class curencyToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Curency",
                table: "CyOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DoubleValue",
                table: "CyKeyData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntValue",
                table: "CyKeyData",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Curency",
                table: "CyOrder");

            migrationBuilder.DropColumn(
                name: "DoubleValue",
                table: "CyKeyData");

            migrationBuilder.DropColumn(
                name: "IntValue",
                table: "CyKeyData");
        }
    }
}
