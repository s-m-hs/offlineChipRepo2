using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class addCommentToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentB",
                table: "CyOrderItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorCommentA",
                table: "CyOrderItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "CyOrderItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "CyOrderItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LevelOk",
                table: "CyOrderItem",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentB",
                table: "CyOrderItem");

            migrationBuilder.DropColumn(
                name: "CreatorCommentA",
                table: "CyOrderItem");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "CyOrderItem");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "CyOrderItem");

            migrationBuilder.DropColumn(
                name: "LevelOk",
                table: "CyOrderItem");
        }
    }
}
