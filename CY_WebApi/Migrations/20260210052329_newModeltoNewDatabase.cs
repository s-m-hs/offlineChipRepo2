using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class newModeltoNewDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CyOrder_CyPcbForm_CyPcbFormID",
                table: "CyOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_CyOrderMessage_CyOrder_OrderID",
                table: "CyOrderMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_CyOrderMessage_CyTicket_TicketID",
                table: "CyOrderMessage");

            migrationBuilder.DropTable(
                name: "CyGuarantee");

            migrationBuilder.DropTable(
                name: "CyInspectionItem");

            migrationBuilder.DropTable(
                name: "CyPcbForm");

            migrationBuilder.DropTable(
                name: "CyProductPriceAndWarranty");

            migrationBuilder.DropTable(
                name: "CyTicket");

            migrationBuilder.DropTable(
                name: "CyInspectionForm");

            migrationBuilder.DropIndex(
                name: "IX_CyOrderMessage_OrderID",
                table: "CyOrderMessage");

            migrationBuilder.DropIndex(
                name: "IX_CyOrderMessage_TicketID",
                table: "CyOrderMessage");

            migrationBuilder.DropIndex(
                name: "IX_CyOrder_CyPcbFormID",
                table: "CyOrder");

            migrationBuilder.DropColumn(
                name: "SeenDate",
                table: "CyOrderMessage");

            migrationBuilder.DropColumn(
                name: "TicketID",
                table: "CyOrderMessage");

            migrationBuilder.DropColumn(
                name: "CyPcbFormID",
                table: "CyOrder");

            migrationBuilder.DropColumn(
                name: "Notification",
                table: "CyOrder");

            migrationBuilder.DropColumn(
                name: "OrderMode",
                table: "CyOrder");

            migrationBuilder.DropColumn(
                name: "StatusText",
                table: "CyOrder");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "CyOrderMessage",
                newName: "MessageType");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CyOrderMessage",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "InProcessDate",
                table: "CyOrder",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "FinalizedDate",
                table: "CyOrder",
                newName: "SendDate");

            migrationBuilder.AddColumn<Guid>(
                name: "CyPCBOrderID",
                table: "OrderItemApproval",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Mentions",
                table: "CyOrderMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "CyOrderMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "InquiryDate",
                table: "CyOrder",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CyPCBOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InquiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderCode = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    CyVahedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyGoroohId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PCBFiles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMechanizedAssembly = table.Column<bool>(type: "bit", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyPCBOrder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyPCBOrder_CyGorooh_CyGoroohId",
                        column: x => x.CyGoroohId,
                        principalTable: "CyGorooh",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyPCBOrder_CyUser_CyUserID",
                        column: x => x.CyUserID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyPCBOrder_CyVahed_CyVahedId",
                        column: x => x.CyVahedId,
                        principalTable: "CyVahed",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyUserOrders",
                columns: table => new
                {
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyUserOrders", x => new { x.UserID, x.OrderID });
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemApproval_CyPCBOrderID",
                table: "OrderItemApproval",
                column: "CyPCBOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_CyPCBOrder_CyGoroohId",
                table: "CyPCBOrder",
                column: "CyGoroohId");

            migrationBuilder.CreateIndex(
                name: "IX_CyPCBOrder_CyUserID",
                table: "CyPCBOrder",
                column: "CyUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CyPCBOrder_CyVahedId",
                table: "CyPCBOrder",
                column: "CyVahedId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemApproval_CyPCBOrder_CyPCBOrderID",
                table: "OrderItemApproval",
                column: "CyPCBOrderID",
                principalTable: "CyPCBOrder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemApproval_CyPCBOrder_CyPCBOrderID",
                table: "OrderItemApproval");

            migrationBuilder.DropTable(
                name: "CyPCBOrder");

            migrationBuilder.DropTable(
                name: "CyUserOrders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemApproval_CyPCBOrderID",
                table: "OrderItemApproval");

            migrationBuilder.DropColumn(
                name: "CyPCBOrderID",
                table: "OrderItemApproval");

            migrationBuilder.DropColumn(
                name: "Mentions",
                table: "CyOrderMessage");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "CyOrderMessage");

            migrationBuilder.DropColumn(
                name: "InquiryDate",
                table: "CyOrder");

            migrationBuilder.RenameColumn(
                name: "MessageType",
                table: "CyOrderMessage",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "CyOrderMessage",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "CyOrder",
                newName: "InProcessDate");

            migrationBuilder.RenameColumn(
                name: "SendDate",
                table: "CyOrder",
                newName: "FinalizedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "SeenDate",
                table: "CyOrderMessage",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketID",
                table: "CyOrderMessage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CyPcbFormID",
                table: "CyOrder",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Notification",
                table: "CyOrder",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderMode",
                table: "CyOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusText",
                table: "CyOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CyGuarantee",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyExplaination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuaranteeCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuaranteeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuarantreePrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductProblem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecievedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyGuarantee", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyGuarantee_CyUser_CyUserID",
                        column: x => x.CyUserID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyInspectionForm",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyAddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Baking = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExternalVisualInspection = table.Column<int>(type: "int", nullable: true),
                    File = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HeatedChemicalTest = table.Column<int>(type: "int", nullable: true),
                    InternalVisualInspection = table.Column<int>(type: "int", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    KeyFunctional = table.Column<int>(type: "int", nullable: true),
                    Lab = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PinCorrelationTest = table.Column<int>(type: "int", nullable: true),
                    ProgrammingTest = table.Column<int>(type: "int", nullable: true),
                    Radiography = table.Column<int>(type: "int", nullable: true),
                    SolderabilityAnalysis = table.Column<int>(type: "int", nullable: true),
                    TapeAndReel = table.Column<int>(type: "int", nullable: true),
                    XRFTest = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyInspectionForm", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyInspectionForm_CyAddress_CyAddressID",
                        column: x => x.CyAddressID,
                        principalTable: "CyAddress",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyInspectionForm_CyUser_UserID",
                        column: x => x.UserID,
                        principalTable: "CyUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CyPcbForm",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardThickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BomExcell = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CuThicknessBottom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuThicknessLayers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuThicknessPlanes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuThicknessTop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuttingLayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPC_A_610_G = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPCstandardIPC_A_600 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Layers = table.Column<int>(type: "int", nullable: false),
                    MechanizedَََAssembly = table.Column<bool>(type: "bit", nullable: false),
                    PCBmaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCBquantity = table.Column<int>(type: "int", nullable: false),
                    PCBquantityTwo = table.Column<int>(type: "int", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceAndPick = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SilkScreenColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SilkScreenLayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolderMask = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolderMaskColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolderMaskLayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolderMaskThickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoliderPaste = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackedLayers = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SurfaceFinish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViaFilling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipFile = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyPcbForm", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyPcbForm_CyUser_UserID",
                        column: x => x.UserID,
                        principalTable: "CyUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CyProductPriceAndWarranty",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Availability = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoOffUnitPrice = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyProductPriceAndWarranty", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyProductPriceAndWarranty_CyProduct_CyProductId",
                        column: x => x.CyProductId,
                        principalTable: "CyProduct",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CyTicket",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyTicket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyTicket_CyUser_UserId",
                        column: x => x.UserId,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyInspectionItem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CyInspectionFormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TargetDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyInspectionItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyInspectionItem_CyInspectionForm_CyInspectionFormID",
                        column: x => x.CyInspectionFormID,
                        principalTable: "CyInspectionForm",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyOrderMessage_OrderID",
                table: "CyOrderMessage",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrderMessage_TicketID",
                table: "CyOrderMessage",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrder_CyPcbFormID",
                table: "CyOrder",
                column: "CyPcbFormID");

            migrationBuilder.CreateIndex(
                name: "IX_CyGuarantee_CyUserID",
                table: "CyGuarantee",
                column: "CyUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CyGuarantee_GuaranteeID",
                table: "CyGuarantee",
                column: "GuaranteeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CyInspectionForm_CyAddressID",
                table: "CyInspectionForm",
                column: "CyAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_CyInspectionForm_UserID",
                table: "CyInspectionForm",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CyInspectionItem_CyInspectionFormID",
                table: "CyInspectionItem",
                column: "CyInspectionFormID");

            migrationBuilder.CreateIndex(
                name: "IX_CyPcbForm_UserID",
                table: "CyPcbForm",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CyProductPriceAndWarranty_CyProductId",
                table: "CyProductPriceAndWarranty",
                column: "CyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CyTicket_UserId",
                table: "CyTicket",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CyOrder_CyPcbForm_CyPcbFormID",
                table: "CyOrder",
                column: "CyPcbFormID",
                principalTable: "CyPcbForm",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CyOrderMessage_CyOrder_OrderID",
                table: "CyOrderMessage",
                column: "OrderID",
                principalTable: "CyOrder",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CyOrderMessage_CyTicket_TicketID",
                table: "CyOrderMessage",
                column: "TicketID",
                principalTable: "CyTicket",
                principalColumn: "ID");
        }
    }
}
