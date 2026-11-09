using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class pcbOrderEdite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemApproval_CyPCBOrder_CyPCBOrderID",
                table: "OrderItemApproval");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemApproval_CyPCBOrderID",
                table: "OrderItemApproval");

            migrationBuilder.DropColumn(
                name: "CyPCBOrderID",
                table: "OrderItemApproval");

            migrationBuilder.AddColumn<string>(
                name: "CommentB",
                table: "CyPCBOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorComment",
                table: "CyPCBOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "CyPCBOrder",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "CyPCBOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "CyPCBOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "CyPCBOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PCBFile",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCBFile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PCBFile_CyUser_CyUserID",
                        column: x => x.CyUserID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PCBFile_CyUserID",
                table: "PCBFile",
                column: "CyUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PCBFile");

            migrationBuilder.DropColumn(
                name: "CommentB",
                table: "CyPCBOrder");

            migrationBuilder.DropColumn(
                name: "CreatorComment",
                table: "CyPCBOrder");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "CyPCBOrder");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "CyPCBOrder");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "CyPCBOrder");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "CyPCBOrder");

            migrationBuilder.AddColumn<Guid>(
                name: "CyPCBOrderID",
                table: "OrderItemApproval",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemApproval_CyPCBOrderID",
                table: "OrderItemApproval",
                column: "CyPCBOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemApproval_CyPCBOrder_CyPCBOrderID",
                table: "OrderItemApproval",
                column: "CyPCBOrderID",
                principalTable: "CyPCBOrder",
                principalColumn: "ID");
        }
    }
}
