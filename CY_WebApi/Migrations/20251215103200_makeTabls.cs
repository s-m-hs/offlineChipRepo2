using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CY_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class makeTabls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessTable",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowCreateOrdere = table.Column<bool>(type: "bit", nullable: false),
                    AllowSeePriceOrder_OrderParts = table.Column<bool>(type: "bit", nullable: false),
                    AllowRequestInitialinquiry = table.Column<bool>(type: "bit", nullable: false),
                    AllowInviteToChat = table.Column<bool>(type: "bit", nullable: false),
                    AllowSeeOrders = table.Column<bool>(type: "bit", nullable: false),
                    AllowUploudInitialinquiry = table.Column<bool>(type: "bit", nullable: false),
                    AllowSentOrdersForInquiry = table.Column<bool>(type: "bit", nullable: false),
                    AllowSetCurrentOrdersStatusA = table.Column<bool>(type: "bit", nullable: false),
                    AllowSetCurrentOrdersStatusB = table.Column<bool>(type: "bit", nullable: false),
                    AllowAccessConfirmedOrdersForSupply = table.Column<bool>(type: "bit", nullable: false),
                    AllowAccessDeliveredOrders = table.Column<bool>(type: "bit", nullable: false),
                    AllowAccessCanceledOrders = table.Column<bool>(type: "bit", nullable: false),
                    AllowAccessAllOrders = table.Column<bool>(type: "bit", nullable: false),
                    AllowCanclePurchaseRequest = table.Column<bool>(type: "bit", nullable: false),
                    AllowUploudWarehouse = table.Column<bool>(type: "bit", nullable: false),
                    AllowAccessPurchasePanel = table.Column<bool>(type: "bit", nullable: false),
                    AllowAccessUsersPanel = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessTable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CyGorooh",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyGorooh", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CyKeyData",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyKeyData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CyListMail",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyListMail", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CyManufacturer",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyManufacturer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CyMenus",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyMenus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CyProductCategory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderValue = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCount = table.Column<int>(type: "int", nullable: false),
                    RootId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyProductCategory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyProductCategory_CyProductCategory_RootId",
                        column: x => x.RootId,
                        principalTable: "CyProductCategory",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyProfile",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyProfile", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CySkin",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "CyVahed",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyVahed", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CyCategory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderValue = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCount = table.Column<int>(type: "int", nullable: false),
                    CySkinId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    rootId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "CyUser",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUsNm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CyHsPs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MaxWrongPass = table.Column<int>(type: "int", nullable: false),
                    WrongPass = table.Column<int>(type: "int", nullable: false),
                    TTKK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CyProfileID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccessTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyGoroohID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyVahedID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userType = table.Column<int>(type: "int", nullable: false),
                    LastLoginTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegisterCode = table.Column<int>(type: "int", nullable: false),
                    RegisterCodeTimer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    rootId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyUser", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyUser_AccessTable_AccessTableID",
                        column: x => x.AccessTableID,
                        principalTable: "AccessTable",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyUser_CyGorooh_CyGoroohID",
                        column: x => x.CyGoroohID,
                        principalTable: "CyGorooh",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyUser_CyProfile_CyProfileID",
                        column: x => x.CyProfileID,
                        principalTable: "CyProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyUser_CyUser_rootId",
                        column: x => x.rootId,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyUser_CyVahed_CyVahedID",
                        column: x => x.CyVahedID,
                        principalTable: "CyVahed",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CyMenuItem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderValue = table.Column<int>(type: "int", nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CyCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CySkinId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isProduct = table.Column<bool>(type: "bit", nullable: true),
                    rootId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Meta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyMenuItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyMenuItem_CyCategory_CyCategoryId",
                        column: x => x.CyCategoryId,
                        principalTable: "CyCategory",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyMenuItem_CyMenuItem_rootId",
                        column: x => x.rootId,
                        principalTable: "CyMenuItem",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyMenuItem_CyMenus_CyMenuId",
                        column: x => x.CyMenuId,
                        principalTable: "CyMenus",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyMenuItem_CySkin_CySkinId",
                        column: x => x.CySkinId,
                        principalTable: "CySkin",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyProduct",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    NoOffPrice = table.Column<double>(type: "float", nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MfrNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatasheetUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supply = table.Column<int>(type: "int", nullable: false),
                    MainImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CyCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyManufacturerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyProduct", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyProduct_CyCategory_CyCategoryId",
                        column: x => x.CyCategoryId,
                        principalTable: "CyCategory",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyProduct_CyManufacturer_CyManufacturerId",
                        column: x => x.CyManufacturerId,
                        principalTable: "CyManufacturer",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyProduct_CyProductCategory_CyProductCategoryId",
                        column: x => x.CyProductCategoryId,
                        principalTable: "CyProductCategory",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyAddress",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyAddress", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyAddress_CyUser_CyUserID",
                        column: x => x.CyUserID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyFile",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileIQ = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyFile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyFile_CyUser_SenderID",
                        column: x => x.SenderID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyGuarantee",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuaranteeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuaranteeCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuarantreePrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecievedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductProblem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyExplaination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "CyPcbForm",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPCstandardIPC_A_600 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCBquantity = table.Column<int>(type: "int", nullable: false),
                    PCBmaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardThickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuThicknessTop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuThicknessBottom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuThicknessPlanes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuThicknessLayers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Layers = table.Column<int>(type: "int", nullable: false),
                    CuttingLayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurfaceFinish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolderMask = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolderMaskLayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolderMaskThickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SilkScreenLayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SilkScreenColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViaFilling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolderMaskColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipFile = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MechanizedَََAssembly = table.Column<bool>(type: "bit", nullable: false),
                    IPC_A_610_G = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoliderPaste = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BomExcell = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlaceAndPick = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DescriptionOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackedLayers = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PCBquantityTwo = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "CySubject",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URL_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Describtion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CySkinId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateShow = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateExp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAuthenticate = table.Column<bool>(type: "bit", nullable: false),
                    BigImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderValue = table.Column<int>(type: "int", nullable: false),
                    EditorState = table.Column<int>(type: "int", nullable: true),
                    bodyString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CyCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CySubject", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CySubject_CyCategory_CyCategoryId",
                        column: x => x.CyCategoryId,
                        principalTable: "CyCategory",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CySubject_CySkin_CySkinId",
                        column: x => x.CySkinId,
                        principalTable: "CySkin",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CySubject_CyUser_CreateById",
                        column: x => x.CreateById,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyTicket",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "CyProductPriceAndWarranty",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<int>(type: "int", nullable: true),
                    NoOffUnitPrice = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Availability = table.Column<int>(type: "int", nullable: false),
                    CyProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "CyProductSpec",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CyProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyProductSpec", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyProductSpec_CyProductCategory_CyProductCategoryId",
                        column: x => x.CyProductCategoryId,
                        principalTable: "CyProductCategory",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyProductSpec_CyProduct_CyProductId",
                        column: x => x.CyProductId,
                        principalTable: "CyProduct",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CyInspectionForm",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lab = table.Column<int>(type: "int", nullable: true),
                    ExternalVisualInspection = table.Column<int>(type: "int", nullable: true),
                    PinCorrelationTest = table.Column<int>(type: "int", nullable: true),
                    ProgrammingTest = table.Column<int>(type: "int", nullable: true),
                    SolderabilityAnalysis = table.Column<int>(type: "int", nullable: true),
                    Radiography = table.Column<int>(type: "int", nullable: true),
                    XRFTest = table.Column<int>(type: "int", nullable: true),
                    KeyFunctional = table.Column<int>(type: "int", nullable: true),
                    Baking = table.Column<int>(type: "int", nullable: true),
                    TapeAndReel = table.Column<int>(type: "int", nullable: true),
                    InternalVisualInspection = table.Column<int>(type: "int", nullable: true),
                    HeatedChemicalTest = table.Column<int>(type: "int", nullable: true),
                    CyAddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    File = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "CyOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderMode = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalizedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderCode = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    CyPcbFormID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notification = table.Column<bool>(type: "bit", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyOrder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyOrder_CyPcbForm_CyPcbFormID",
                        column: x => x.CyPcbFormID,
                        principalTable: "CyPcbForm",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyOrder_CyUser_CyUserID",
                        column: x => x.CyUserID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CySub_Cats",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CySubjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyCategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CySub_Cats", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CySub_Cats_CyCategory_CyCategoryID",
                        column: x => x.CyCategoryID,
                        principalTable: "CyCategory",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CySub_Cats_CySubject_CySubjectID",
                        column: x => x.CySubjectID,
                        principalTable: "CySubject",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyInspectionItem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CyInspectionFormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TargetDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "CyOrderItem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CyProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CyOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyOrderItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyOrderItem_CyOrder_CyOrderID",
                        column: x => x.CyOrderID,
                        principalTable: "CyOrder",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyOrderItem_CyProduct_CyProductID",
                        column: x => x.CyProductID,
                        principalTable: "CyProduct",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CyOrderMessage",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TicketID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SeenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyOrderMessage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CyOrderMessage_CyOrder_OrderID",
                        column: x => x.OrderID,
                        principalTable: "CyOrder",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyOrderMessage_CyTicket_TicketID",
                        column: x => x.TicketID,
                        principalTable: "CyTicket",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CyOrderMessage_CyUser_SenderID",
                        column: x => x.SenderID,
                        principalTable: "CyUser",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CyAddress_CyUserID",
                table: "CyAddress",
                column: "CyUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CyCategory_CySkinId",
                table: "CyCategory",
                column: "CySkinId");

            migrationBuilder.CreateIndex(
                name: "IX_CyCategory_rootId",
                table: "CyCategory",
                column: "rootId");

            migrationBuilder.CreateIndex(
                name: "IX_CyFile_SenderID",
                table: "CyFile",
                column: "SenderID");

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
                name: "IX_CyMenuItem_CyCategoryId",
                table: "CyMenuItem",
                column: "CyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CyMenuItem_CyMenuId",
                table: "CyMenuItem",
                column: "CyMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_CyMenuItem_CySkinId",
                table: "CyMenuItem",
                column: "CySkinId");

            migrationBuilder.CreateIndex(
                name: "IX_CyMenuItem_rootId",
                table: "CyMenuItem",
                column: "rootId");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrder_CyPcbFormID",
                table: "CyOrder",
                column: "CyPcbFormID");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrder_CyUserID",
                table: "CyOrder",
                column: "CyUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrderItem_CyOrderID",
                table: "CyOrderItem",
                column: "CyOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrderItem_CyProductID",
                table: "CyOrderItem",
                column: "CyProductID");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrderMessage_OrderID",
                table: "CyOrderMessage",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrderMessage_SenderID",
                table: "CyOrderMessage",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_CyOrderMessage_TicketID",
                table: "CyOrderMessage",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_CyPcbForm_UserID",
                table: "CyPcbForm",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CyProduct_CyCategoryId",
                table: "CyProduct",
                column: "CyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CyProduct_CyManufacturerId",
                table: "CyProduct",
                column: "CyManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_CyProduct_CyProductCategoryId",
                table: "CyProduct",
                column: "CyProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CyProductCategory_RootId",
                table: "CyProductCategory",
                column: "RootId");

            migrationBuilder.CreateIndex(
                name: "IX_CyProductPriceAndWarranty_CyProductId",
                table: "CyProductPriceAndWarranty",
                column: "CyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CyProductSpec_CyProductCategoryId",
                table: "CyProductSpec",
                column: "CyProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CyProductSpec_CyProductId",
                table: "CyProductSpec",
                column: "CyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CySub_Cats_CyCategoryID",
                table: "CySub_Cats",
                column: "CyCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_CySub_Cats_CySubjectID",
                table: "CySub_Cats",
                column: "CySubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_CySubject_CreateById",
                table: "CySubject",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_CySubject_CyCategoryId",
                table: "CySubject",
                column: "CyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CySubject_CySkinId",
                table: "CySubject",
                column: "CySkinId");

            migrationBuilder.CreateIndex(
                name: "IX_CyTicket_UserId",
                table: "CyTicket",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CyUser_AccessTableID",
                table: "CyUser",
                column: "AccessTableID");

            migrationBuilder.CreateIndex(
                name: "IX_CyUser_CyGoroohID",
                table: "CyUser",
                column: "CyGoroohID");

            migrationBuilder.CreateIndex(
                name: "IX_CyUser_CyProfileID",
                table: "CyUser",
                column: "CyProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_CyUser_CyVahedID",
                table: "CyUser",
                column: "CyVahedID");

            migrationBuilder.CreateIndex(
                name: "IX_CyUser_rootId",
                table: "CyUser",
                column: "rootId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyFile");

            migrationBuilder.DropTable(
                name: "CyGuarantee");

            migrationBuilder.DropTable(
                name: "CyInspectionItem");

            migrationBuilder.DropTable(
                name: "CyKeyData");

            migrationBuilder.DropTable(
                name: "CyListMail");

            migrationBuilder.DropTable(
                name: "CyMenuItem");

            migrationBuilder.DropTable(
                name: "CyOrderItem");

            migrationBuilder.DropTable(
                name: "CyOrderMessage");

            migrationBuilder.DropTable(
                name: "CyProductPriceAndWarranty");

            migrationBuilder.DropTable(
                name: "CyProductSpec");

            migrationBuilder.DropTable(
                name: "CySub_Cats");

            migrationBuilder.DropTable(
                name: "CyInspectionForm");

            migrationBuilder.DropTable(
                name: "CyMenus");

            migrationBuilder.DropTable(
                name: "CyOrder");

            migrationBuilder.DropTable(
                name: "CyTicket");

            migrationBuilder.DropTable(
                name: "CyProduct");

            migrationBuilder.DropTable(
                name: "CySubject");

            migrationBuilder.DropTable(
                name: "CyAddress");

            migrationBuilder.DropTable(
                name: "CyPcbForm");

            migrationBuilder.DropTable(
                name: "CyManufacturer");

            migrationBuilder.DropTable(
                name: "CyProductCategory");

            migrationBuilder.DropTable(
                name: "CyCategory");

            migrationBuilder.DropTable(
                name: "CyUser");

            migrationBuilder.DropTable(
                name: "CySkin");

            migrationBuilder.DropTable(
                name: "AccessTable");

            migrationBuilder.DropTable(
                name: "CyGorooh");

            migrationBuilder.DropTable(
                name: "CyProfile");

            migrationBuilder.DropTable(
                name: "CyVahed");
        }
    }
}
