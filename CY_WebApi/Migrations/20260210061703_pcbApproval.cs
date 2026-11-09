using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class pcbApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemApproval_CyPCBOrder_CyPCBOrderID",
                table: "OrderItemApproval");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "CyOrder");

            migrationBuilder.AlterColumn<Guid>(
                name: "CyPCBOrderID",
                table: "OrderItemApproval",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "PCBOrderItemApproval",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyPCBOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFirst = table.Column<bool>(type: "bit", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCBOrderItemApproval", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PCBOrderItemApproval_CyPCBOrder_CyPCBOrderID",
                        column: x => x.CyPCBOrderID,
                        principalTable: "CyPCBOrder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCBOrderItemApproval_CyUser_CyUserID",
                        column: x => x.CyUserID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PCBOrderItemApproval_CyPCBOrderID",
                table: "PCBOrderItemApproval",
                column: "CyPCBOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PCBOrderItemApproval_CyUserID",
                table: "PCBOrderItemApproval",
                column: "CyUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemApproval_CyPCBOrder_CyPCBOrderID",
                table: "OrderItemApproval",
                column: "CyPCBOrderID",
                principalTable: "CyPCBOrder",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemApproval_CyPCBOrder_CyPCBOrderID",
                table: "OrderItemApproval");

            migrationBuilder.DropTable(
                name: "PCBOrderItemApproval");

            migrationBuilder.AlterColumn<Guid>(
                name: "CyPCBOrderID",
                table: "OrderItemApproval",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "CyOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemApproval_CyPCBOrder_CyPCBOrderID",
                table: "OrderItemApproval",
                column: "CyPCBOrderID",
                principalTable: "CyPCBOrder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
