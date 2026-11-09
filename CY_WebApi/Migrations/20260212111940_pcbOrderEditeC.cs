using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class pcbOrderEditeC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "PCBFile",
                newName: "CyPCBOrderID");

            migrationBuilder.RenameColumn(
                name: "PCBFiles",
                table: "CyPCBOrder",
                newName: "GuidFileList");

            migrationBuilder.CreateIndex(
                name: "IX_PCBFile_CyPCBOrderID",
                table: "PCBFile",
                column: "CyPCBOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBFile_CyPCBOrder_CyPCBOrderID",
                table: "PCBFile",
                column: "CyPCBOrderID",
                principalTable: "CyPCBOrder",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCBFile_CyPCBOrder_CyPCBOrderID",
                table: "PCBFile");

            migrationBuilder.DropIndex(
                name: "IX_PCBFile_CyPCBOrderID",
                table: "PCBFile");

            migrationBuilder.RenameColumn(
                name: "CyPCBOrderID",
                table: "PCBFile",
                newName: "OrderID");

            migrationBuilder.RenameColumn(
                name: "GuidFileList",
                table: "CyPCBOrder",
                newName: "PCBFiles");
        }
    }
}
