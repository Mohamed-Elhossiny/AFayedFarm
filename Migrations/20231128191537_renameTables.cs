using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class renameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "SupplyCategories");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Export = table.Column<int>(type: "int", nullable: true),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    GetPaied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StoreID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientID);
                    table.ForeignKey(
                        name: "FK_Clients_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    FarmsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplyDate = table.Column<DateTime>(type: "Date", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Paied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FarmsNotes = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    StoreID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.FarmsID);
                    table.ForeignKey(
                        name: "FK_Farms_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "FarmsCategories",
                columns: table => new
                {
                    FarmsID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmsCategories", x => new { x.FarmsID, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_FarmsCategories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmsCategories_Farms_FarmsID",
                        column: x => x.FarmsID,
                        principalTable: "Farms",
                        principalColumn: "FarmsID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_StoreID",
                table: "Clients",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_StoreID",
                table: "Farms",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmsCategories_CategoryID",
                table: "FarmsCategories",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "FarmsCategories");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreID = table.Column<int>(type: "int", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Export = table.Column<int>(type: "int", nullable: true),
                    GetPaied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Customers_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreID = table.Column<int>(type: "int", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Paied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierNotes = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    SupplyDate = table.Column<DateTime>(type: "Date", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                    table.ForeignKey(
                        name: "FK_Suppliers_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "SupplyCategories",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyCategories", x => new { x.SupplierID, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_SupplyCategories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplyCategories_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_StoreID",
                table: "Customers",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_StoreID",
                table: "Suppliers",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyCategories_CategoryID",
                table: "SupplyCategories",
                column: "CategoryID");
        }
    }
}
