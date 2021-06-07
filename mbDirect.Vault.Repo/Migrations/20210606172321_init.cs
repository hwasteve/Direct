using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mbDirect.Vault.Repo.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountStatuses",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMaintDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatuses", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMaintDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstrumentTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AddDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expiration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountStatusCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountStatuses_AccountStatusCode",
                        column: x => x.AccountStatusCode,
                        principalTable: "AccountStatuses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_InstrumentTypes_InstrumentTypeCode",
                        column: x => x.InstrumentTypeCode,
                        principalTable: "InstrumentTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "InstrumentTypes",
                columns: new[] { "Code", "Name" },
                values: new object[] { "credit", "Credit Card" });

            migrationBuilder.InsertData(
                table: "InstrumentTypes",
                columns: new[] { "Code", "Name" },
                values: new object[] { "debit", "Debit Card" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountStatusCode",
                table: "Accounts",
                column: "AccountStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_InstrumentTypeCode",
                table: "Accounts",
                column: "InstrumentTypeCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountStatuses");

            migrationBuilder.DropTable(
                name: "InstrumentTypes");
        }
    }
}
