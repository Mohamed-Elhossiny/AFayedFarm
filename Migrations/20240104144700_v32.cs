using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FridgeID",
                table: "SafeTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fridges",
                columns: table => new
                {
                    FridgeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FridgeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "Date", nullable: true, defaultValueSql: "GETDATE()"),
                    TotalRemaining = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fridges", x => x.FridgeID);
                });

            migrationBuilder.CreateTable(
                name: "FridgeProducts",
                columns: table => new
                {
                    FridgeProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FridgeID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplyDate = table.Column<DateTime>(type: "Date", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "Date", nullable: true, defaultValueSql: "GETDATE()"),
                    Number = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Payed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FridgeNotes = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CarNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FridgeProducts", x => x.FridgeProductID);
                    table.ForeignKey(
                        name: "FK_FridgeProducts_Fridges_FridgeID",
                        column: x => x.FridgeID,
                        principalTable: "Fridges",
                        principalColumn: "FridgeID");
                    table.ForeignKey(
                        name: "FK_FridgeProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SafeTransactions_FridgeID",
                table: "SafeTransactions",
                column: "FridgeID");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeProducts_FridgeID",
                table: "FridgeProducts",
                column: "FridgeID");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeProducts_ProductID",
                table: "FridgeProducts",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_SafeTransactions_Fridges_FridgeID",
                table: "SafeTransactions",
                column: "FridgeID",
                principalTable: "Fridges",
                principalColumn: "FridgeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafeTransactions_Fridges_FridgeID",
                table: "SafeTransactions");

            migrationBuilder.DropTable(
                name: "FridgeProducts");

            migrationBuilder.DropTable(
                name: "Fridges");

            migrationBuilder.DropIndex(
                name: "IX_SafeTransactions_FridgeID",
                table: "SafeTransactions");

            migrationBuilder.DropColumn(
                name: "FridgeID",
                table: "SafeTransactions");
        }
    }
}
