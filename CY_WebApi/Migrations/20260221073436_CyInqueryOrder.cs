using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class CyInqueryOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CyInquiryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestFile = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResponseFile = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommentB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyInquiryOrder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyInquiryOrder_CyUser_CyUserID",
                        column: x => x.CyUserID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyInquiryOrder_CyUserID",
                table: "CyInquiryOrder",
                column: "CyUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyInquiryOrder");
        }
    }
}
