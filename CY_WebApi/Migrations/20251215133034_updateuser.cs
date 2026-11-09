using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class updateuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CyUser_CyVahed_CyVahedID",
                table: "CyUser");

            migrationBuilder.AlterColumn<Guid>(
                name: "CyVahedID",
                table: "CyUser",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_CyUser_CyVahed_CyVahedID",
                table: "CyUser",
                column: "CyVahedID",
                principalTable: "CyVahed",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CyUser_CyVahed_CyVahedID",
                table: "CyUser");

            migrationBuilder.AlterColumn<Guid>(
                name: "CyVahedID",
                table: "CyUser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CyUser_CyVahed_CyVahedID",
                table: "CyUser",
                column: "CyVahedID",
                principalTable: "CyVahed",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
