using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gr_Api.Migrations
{
    /// <inheritdoc />
    public partial class FirstInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrCategory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderValue = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCount = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GrManufacturer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrManufacturer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GrProduct",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatasheetUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmartImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFullyGet = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GrManufacturerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrProduct", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GrProduct_GrManufacturer_GrManufacturerID",
                        column: x => x.GrManufacturerID,
                        principalTable: "GrManufacturer",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "GrKeyValue",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrProductID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrKeyValue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GrKeyValue_GrProduct_GrProductID",
                        column: x => x.GrProductID,
                        principalTable: "GrProduct",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrKeyValue_GrProductID",
                table: "GrKeyValue",
                column: "GrProductID");

            migrationBuilder.CreateIndex(
                name: "IX_GrProduct_GrManufacturerID",
                table: "GrProduct",
                column: "GrManufacturerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrCategory");

            migrationBuilder.DropTable(
                name: "GrKeyValue");

            migrationBuilder.DropTable(
                name: "GrProduct");

            migrationBuilder.DropTable(
                name: "GrManufacturer");
        }
    }
}
