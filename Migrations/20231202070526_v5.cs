using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Stores_StoreID",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_Stores_StoreID",
                table: "Farms");

            migrationBuilder.DropForeignKey(
                name: "FK_FarmsProducts_Farms_FarmsID",
                table: "FarmsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FarmsProducts_Products_ProductID",
                table: "FarmsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Stores_StoreID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_StoreID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FarmsProducts",
                table: "FarmsProducts");

            migrationBuilder.DropIndex(
                name: "IX_Farms_StoreID",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Clients_StoreID",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "StoreID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "FarmsNotes",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "NetQuantity",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Paied",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Remaining",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "StoreID",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "SupplyDate",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "GetPaied",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Remaining",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "StoreID",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "FarmsProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FarmsID",
                table: "FarmsProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FarmProductID",
                table: "FarmsProducts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "FarmsProducts",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "FarmsProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FarmsNotes",
                table: "FarmsProducts",
                type: "nvarchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetQuantity",
                table: "FarmsProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Paied",
                table: "FarmsProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "FarmsProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Remaining",
                table: "FarmsProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SupplyDate",
                table: "FarmsProducts",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "FarmsProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FarmsProducts",
                table: "FarmsProducts",
                column: "FarmProductID");

            migrationBuilder.CreateTable(
                name: "StoreProducts",
                columns: table => new
                {
                    StoreProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmsID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    StoreID = table.Column<int>(type: "int", nullable: true),
                    SupplyDate = table.Column<DateTime>(type: "Date", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "Date", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProducts", x => x.StoreProductID);
                    table.ForeignKey(
                        name: "FK_StoreProducts_Farms_FarmsID",
                        column: x => x.FarmsID,
                        principalTable: "Farms",
                        principalColumn: "FarmsID");
                    table.ForeignKey(
                        name: "FK_StoreProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                    table.ForeignKey(
                        name: "FK_StoreProducts_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreID = table.Column<int>(type: "int", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "Date", nullable: true),
                    GetPaied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ClientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmsProducts_FarmsID",
                table: "FarmsProducts",
                column: "FarmsID");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProducts_FarmsID",
                table: "StoreProducts",
                column: "FarmsID");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProducts_ProductID",
                table: "StoreProducts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProducts_StoreID",
                table: "StoreProducts",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ClientID",
                table: "Transactions",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ProductID",
                table: "Transactions",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StoreID",
                table: "Transactions",
                column: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmsProducts_Farms_FarmsID",
                table: "FarmsProducts",
                column: "FarmsID",
                principalTable: "Farms",
                principalColumn: "FarmsID");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmsProducts_Products_ProductID",
                table: "FarmsProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmsProducts_Farms_FarmsID",
                table: "FarmsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FarmsProducts_Products_ProductID",
                table: "FarmsProducts");

            migrationBuilder.DropTable(
                name: "StoreProducts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FarmsProducts",
                table: "FarmsProducts");

            migrationBuilder.DropIndex(
                name: "IX_FarmsProducts_FarmsID",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "FarmProductID",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "FarmsNotes",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "NetQuantity",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "Paied",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "Remaining",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "SupplyDate",
                table: "FarmsProducts");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "FarmsProducts");

            migrationBuilder.AddColumn<int>(
                name: "StoreID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "FarmsProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FarmsID",
                table: "FarmsProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Farms",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Farms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FarmsNotes",
                table: "Farms",
                type: "nvarchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetQuantity",
                table: "Farms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Paied",
                table: "Farms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "Farms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Remaining",
                table: "Farms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreID",
                table: "Farms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SupplyDate",
                table: "Farms",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Farms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Clients",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GetPaied",
                table: "Clients",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Remaining",
                table: "Clients",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StoreID",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FarmsProducts",
                table: "FarmsProducts",
                columns: new[] { "FarmsID", "ProductID" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreID",
                table: "Products",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_StoreID",
                table: "Farms",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_StoreID",
                table: "Clients",
                column: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Stores_StoreID",
                table: "Clients",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_Stores_StoreID",
                table: "Farms",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmsProducts_Farms_FarmsID",
                table: "FarmsProducts",
                column: "FarmsID",
                principalTable: "Farms",
                principalColumn: "FarmsID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FarmsProducts_Products_ProductID",
                table: "FarmsProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stores_StoreID",
                table: "Products",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "StoreID");
        }
    }
}
