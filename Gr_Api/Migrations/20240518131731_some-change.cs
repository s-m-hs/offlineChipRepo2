using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gr_Api.Migrations
{
    /// <inheritdoc />
    public partial class somechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GrCategoryID",
                table: "GrProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rootId",
                table: "GrCategory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CySkin",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CySkin", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CyCategory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderValue = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCount = table.Column<int>(type: "int", nullable: false),
                    CySkinId = table.Column<int>(type: "int", nullable: true),
                    rootId = table.Column<int>(type: "int", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyCategory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyCategory_CyCategory_rootId",
                        column: x => x.rootId,
                        principalTable: "CyCategory",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyCategory_CySkin_CySkinId",
                        column: x => x.CySkinId,
                        principalTable: "CySkin",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrProduct_GrCategoryID",
                table: "GrProduct",
                column: "GrCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_GrCategory_rootId",
                table: "GrCategory",
                column: "rootId");

            migrationBuilder.CreateIndex(
                name: "IX_CyCategory_CySkinId",
                table: "CyCategory",
                column: "CySkinId");

            migrationBuilder.CreateIndex(
                name: "IX_CyCategory_rootId",
                table: "CyCategory",
                column: "rootId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrCategory_CyCategory_rootId",
                table: "GrCategory",
                column: "rootId",
                principalTable: "CyCategory",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GrProduct_GrCategory_GrCategoryID",
                table: "GrProduct",
                column: "GrCategoryID",
                principalTable: "GrCategory",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrCategory_CyCategory_rootId",
                table: "GrCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_GrProduct_GrCategory_GrCategoryID",
                table: "GrProduct");

            migrationBuilder.DropTable(
                name: "CyCategory");

            migrationBuilder.DropTable(
                name: "CySkin");

            migrationBuilder.DropIndex(
                name: "IX_GrProduct_GrCategoryID",
                table: "GrProduct");

            migrationBuilder.DropIndex(
                name: "IX_GrCategory_rootId",
                table: "GrCategory");

            migrationBuilder.DropColumn(
                name: "GrCategoryID",
                table: "GrProduct");

            migrationBuilder.DropColumn(
                name: "rootId",
                table: "GrCategory");
        }
    }
}
