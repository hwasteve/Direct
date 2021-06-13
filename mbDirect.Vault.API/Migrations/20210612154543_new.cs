using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mbDirect.Vault.API.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountStatuses",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    LastMaintDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatuses", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Gateways",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    EndPoint = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LastMaintDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gateways", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    LastMaintDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    KeyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeyValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Vector = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LastMaintDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstrumentTypeCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    AddDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    OwnerName = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true),
                    Number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    RoutingNumber = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: true),
                    Expiration = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true),
                    BillingZipCode = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true),
                    AccountStatusCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    StatusDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_InstrumentType",
                        column: x => x.InstrumentTypeCode,
                        principalTable: "InstrumentTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Status",
                        column: x => x.AccountStatusCode,
                        principalTable: "AccountStatuses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransitCredentials",
                columns: table => new
                {
                    Number = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    PasswordEncrypted = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PasswordKeyId = table.Column<int>(type: "int", nullable: false),
                    DeveloperId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransitGatewayId = table.Column<int>(type: "int", nullable: true),
                    LastMaintDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitCredentials", x => x.Number);
                    table.ForeignKey(
                        name: "FK_TransitCredential_PasswordKey",
                        column: x => x.PasswordKeyId,
                        principalTable: "Keys",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransitCredentials_Gateways_TransitGatewayId",
                        column: x => x.TransitGatewayId,
                        principalTable: "Gateways",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    MerchantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    TransitUserNumber = table.Column<int>(type: "int", nullable: true),
                    TransitTransactionKey = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    LastMaintDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    TransitCredentialNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.MerchantId);
                    table.ForeignKey(
                        name: "FK_Merchants_TransitCredentials_TransitCredentialNumber",
                        column: x => x.TransitCredentialNumber,
                        principalTable: "TransitCredentials",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Merchants_TransitCredentials_TransitUserNumber",
                        column: x => x.TransitUserNumber,
                        principalTable: "TransitCredentials",
                        principalColumn: "Number",
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

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_TransitCredentialNumber",
                table: "Merchants",
                column: "TransitCredentialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_TransitUserNumber",
                table: "Merchants",
                column: "TransitUserNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TransitCredentials_PasswordKeyId",
                table: "TransitCredentials",
                column: "PasswordKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitCredentials_TransitGatewayId",
                table: "TransitCredentials",
                column: "TransitGatewayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "InstrumentTypes");

            migrationBuilder.DropTable(
                name: "AccountStatuses");

            migrationBuilder.DropTable(
                name: "TransitCredentials");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "Gateways");
        }
    }
}
