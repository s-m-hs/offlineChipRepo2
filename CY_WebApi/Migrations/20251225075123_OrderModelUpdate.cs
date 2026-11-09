using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class OrderModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CyGoroohId",
                table: "CyOrder",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CyVahedId",
                table: "CyOrder",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderItemApproval",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyOrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemApproval", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderItemApproval_CyOrderItem_CyOrderItemID",
                        column: x => x.CyOrderItemID,
                        principalTable: "CyOrderItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemApproval_CyUser_CyUserID",
                        column: x => x.CyUserID,
                        principalTable: "CyUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyOrder_CyGoroohId",
                table: "CyOrder",
                column: "CyGoroohId");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrder_CyVahedId",
                table: "CyOrder",
                column: "CyVahedId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemApproval_CyOrderItemID",
                table: "OrderItemApproval",
                column: "CyOrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemApproval_CyUserID",
                table: "OrderItemApproval",
                column: "CyUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CyOrder_CyGorooh_CyGoroohId",
                table: "CyOrder",
                column: "CyGoroohId",
                principalTable: "CyGorooh",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CyOrder_CyVahed_CyVahedId",
                table: "CyOrder",
                column: "CyVahedId",
                principalTable: "CyVahed",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CyOrder_CyGorooh_CyGoroohId",
                table: "CyOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_CyOrder_CyVahed_CyVahedId",
                table: "CyOrder");

            migrationBuilder.DropTable(
                name: "OrderItemApproval");

            migrationBuilder.DropIndex(
                name: "IX_CyOrder_CyGoroohId",
                table: "CyOrder");

            migrationBuilder.DropIndex(
                name: "IX_CyOrder_CyVahedId",
                table: "CyOrder");

            migrationBuilder.DropColumn(
                name: "CyGoroohId",
                table: "CyOrder");

            migrationBuilder.DropColumn(
                name: "CyVahedId",
                table: "CyOrder");
        }
    }
}
