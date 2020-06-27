using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JemenaGasMeter.WebApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Depots",
                columns: table => new
                {
                    DepotID = table.Column<string>(nullable: false),
                    StreetName = table.Column<string>(nullable: true),
                    Suburb = table.Column<string>(nullable: true),
                    PostCode = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depots", x => x.DepotID);
                });

            migrationBuilder.CreateTable(
                name: "Installations",
                columns: table => new
                {
                    InstallationID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MIRN = table.Column<string>(nullable: true),
                    StreetNo = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true),
                    Suburb = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    PostCode = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installations", x => x.InstallationID);
                });

            migrationBuilder.CreateTable(
                name: "Meters",
                columns: table => new
                {
                    MIRN = table.Column<string>(nullable: false),
                    MeterType = table.Column<int>(nullable: false),
                    MeterStatus = table.Column<int>(nullable: false),
                    MeterCondition = table.Column<int>(nullable: false),
                    ExpriyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meters", x => x.MIRN);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    TransferID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransferID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    PayRollID = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    PIN = table.Column<string>(nullable: true),
                    UserStatus = table.Column<int>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.PayRollID);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseID = table.Column<string>(nullable: false),
                    StreetName = table.Column<string>(nullable: true),
                    Suburb = table.Column<string>(nullable: true),
                    PostCode = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseID);
                });

            migrationBuilder.CreateTable(
                name: "MeterHistories",
                columns: table => new
                {
                    MeterHistoryID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MIRN = table.Column<string>(nullable: true),
                    PayRollID = table.Column<string>(nullable: true),
                    MeterStatus = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    TransfereeID = table.Column<string>(nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterHistories", x => x.MeterHistoryID);
                    table.ForeignKey(
                        name: "FK_MeterHistories_Meters_MIRN",
                        column: x => x.MIRN,
                        principalTable: "Meters",
                        principalColumn: "MIRN",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeterHistories_Users_PayRollID",
                        column: x => x.PayRollID,
                        principalTable: "Users",
                        principalColumn: "PayRollID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterHistories_MIRN",
                table: "MeterHistories",
                column: "MIRN");

            migrationBuilder.CreateIndex(
                name: "IX_MeterHistories_PayRollID",
                table: "MeterHistories",
                column: "PayRollID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "Installations");

            migrationBuilder.DropTable(
                name: "MeterHistories");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Meters");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
