using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class updateGrope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VahedId",
                table: "CyGorooh",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CyGorooh_VahedId",
                table: "CyGorooh",
                column: "VahedId");

            migrationBuilder.AddForeignKey(
                name: "FK_CyGorooh_CyVahed_VahedId",
                table: "CyGorooh",
                column: "VahedId",
                principalTable: "CyVahed",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CyGorooh_CyVahed_VahedId",
                table: "CyGorooh");

            migrationBuilder.DropIndex(
                name: "IX_CyGorooh_VahedId",
                table: "CyGorooh");

            migrationBuilder.DropColumn(
                name: "VahedId",
                table: "CyGorooh");
        }
    }
}
