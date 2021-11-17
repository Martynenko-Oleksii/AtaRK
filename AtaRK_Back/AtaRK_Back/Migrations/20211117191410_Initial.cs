using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AtaRK_Back.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TechMessageAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechMessageAnswers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FastFoodFranchises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinTemperature = table.Column<double>(type: "float", nullable: false),
                    MaxTemperature = table.Column<double>(type: "float", nullable: false),
                    MinHuumidity = table.Column<double>(type: "float", nullable: false),
                    MaxHuumidity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastFoodFranchises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FastFoodFranchises_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopAdmins_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsMaster = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemAdmins_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FranchiseContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPhone = table.Column<bool>(type: "bit", nullable: false),
                    IsEmail = table.Column<bool>(type: "bit", nullable: false),
                    IsUrl = table.Column<bool>(type: "bit", nullable: false),
                    UrlType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FastFoodFranchiseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FranchiseContactInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FranchiseContactInfos_FastFoodFranchises_FastFoodFranchiseId",
                        column: x => x.FastFoodFranchiseId,
                        principalTable: "FastFoodFranchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FranchiseImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBanner = table.Column<bool>(type: "bit", nullable: false),
                    FastFoodFranchiseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FranchiseImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FranchiseImages_FastFoodFranchises_FastFoodFranchiseId",
                        column: x => x.FastFoodFranchiseId,
                        principalTable: "FastFoodFranchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FranchiseShops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FastFoodFranchiseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FranchiseShops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FranchiseShops_FastFoodFranchises_FastFoodFranchiseId",
                        column: x => x.FastFoodFranchiseId,
                        principalTable: "FastFoodFranchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FranchiseShops_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FastFoodFranchiseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopApplications_FastFoodFranchises_FastFoodFranchiseId",
                        column: x => x.FastFoodFranchiseId,
                        principalTable: "FastFoodFranchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClimateDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceNumber = table.Column<int>(type: "int", nullable: false),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    FranchiseShopId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClimateDevices_FranchiseShops_FranchiseShopId",
                        column: x => x.FranchiseShopId,
                        principalTable: "FranchiseShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FranchiseShopShopAdmin",
                columns: table => new
                {
                    FranchiseShopsId = table.Column<int>(type: "int", nullable: false),
                    ShopAdminsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FranchiseShopShopAdmin", x => new { x.FranchiseShopsId, x.ShopAdminsId });
                    table.ForeignKey(
                        name: "FK_FranchiseShopShopAdmin_FranchiseShops_FranchiseShopsId",
                        column: x => x.FranchiseShopsId,
                        principalTable: "FranchiseShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FranchiseShopShopAdmin_ShopAdmins_ShopAdminsId",
                        column: x => x.ShopAdminsId,
                        principalTable: "ShopAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClimateStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Huumidity = table.Column<double>(type: "float", nullable: false),
                    ClimateDeviceId = table.Column<int>(type: "int", nullable: true),
                    FranchiseShopId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClimateStates_ClimateDevices_ClimateDeviceId",
                        column: x => x.ClimateDeviceId,
                        principalTable: "ClimateDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClimateStates_FranchiseShops_FranchiseShopId",
                        column: x => x.FranchiseShopId,
                        principalTable: "FranchiseShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClimateDeviceId = table.Column<int>(type: "int", nullable: true),
                    ShopAdminId = table.Column<int>(type: "int", nullable: true),
                    TechMessageAnswerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechMessages_ClimateDevices_ClimateDeviceId",
                        column: x => x.ClimateDeviceId,
                        principalTable: "ClimateDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechMessages_ShopAdmins_ShopAdminId",
                        column: x => x.ShopAdminId,
                        principalTable: "ShopAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechMessages_TechMessageAnswers_TechMessageAnswerId",
                        column: x => x.TechMessageAnswerId,
                        principalTable: "TechMessageAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDevices_FranchiseShopId",
                table: "ClimateDevices",
                column: "FranchiseShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateStates_ClimateDeviceId",
                table: "ClimateStates",
                column: "ClimateDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateStates_FranchiseShopId",
                table: "ClimateStates",
                column: "FranchiseShopId");

            migrationBuilder.CreateIndex(
                name: "IX_FranchiseContactInfos_FastFoodFranchiseId",
                table: "FranchiseContactInfos",
                column: "FastFoodFranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_FranchiseImages_FastFoodFranchiseId",
                table: "FranchiseImages",
                column: "FastFoodFranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_FranchiseShops_FastFoodFranchiseId",
                table: "FranchiseShops",
                column: "FastFoodFranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_FranchiseShopShopAdmin_ShopAdminsId",
                table: "FranchiseShopShopAdmin",
                column: "ShopAdminsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopApplications_FastFoodFranchiseId",
                table: "ShopApplications",
                column: "FastFoodFranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_TechMessages_ClimateDeviceId",
                table: "TechMessages",
                column: "ClimateDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TechMessages_ShopAdminId",
                table: "TechMessages",
                column: "ShopAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_TechMessages_TechMessageAnswerId",
                table: "TechMessages",
                column: "TechMessageAnswerId",
                unique: true,
                filter: "[TechMessageAnswerId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClimateStates");

            migrationBuilder.DropTable(
                name: "FranchiseContactInfos");

            migrationBuilder.DropTable(
                name: "FranchiseImages");

            migrationBuilder.DropTable(
                name: "FranchiseShopShopAdmin");

            migrationBuilder.DropTable(
                name: "ShopApplications");

            migrationBuilder.DropTable(
                name: "SystemAdmins");

            migrationBuilder.DropTable(
                name: "TechMessages");

            migrationBuilder.DropTable(
                name: "ClimateDevices");

            migrationBuilder.DropTable(
                name: "ShopAdmins");

            migrationBuilder.DropTable(
                name: "TechMessageAnswers");

            migrationBuilder.DropTable(
                name: "FranchiseShops");

            migrationBuilder.DropTable(
                name: "FastFoodFranchises");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
