using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class approvalEdieuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemApproval_CyUser_CyUserID",
                table: "OrderItemApproval");

            migrationBuilder.AlterColumn<Guid>(
                name: "CyUserID",
                table: "OrderItemApproval",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "OrderItemApproval",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemApproval_CyUser_CyUserID",
                table: "OrderItemApproval",
                column: "CyUserID",
                principalTable: "CyUser",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemApproval_CyUser_CyUserID",
                table: "OrderItemApproval");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "OrderItemApproval");

            migrationBuilder.AlterColumn<Guid>(
                name: "CyUserID",
                table: "OrderItemApproval",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemApproval_CyUser_CyUserID",
                table: "OrderItemApproval",
                column: "CyUserID",
                principalTable: "CyUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
