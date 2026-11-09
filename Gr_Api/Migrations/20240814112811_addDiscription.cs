using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gr_Api.Migrations
{
    /// <inheritdoc />
    public partial class addDiscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "GrCategory",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "GrCategory");
        }
    }
}
