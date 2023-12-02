using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmsCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Clients",
                newName: "ProductID");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Stores",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Farms",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Expenses",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Clients",
                type: "Date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductNote = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "Date", nullable: true),
                    StoreID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "FarmsProducts",
                columns: table => new
                {
                    FarmsID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmsProducts", x => new { x.FarmsID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_FarmsProducts_Farms_FarmsID",
                        column: x => x.FarmsID,
                        principalTable: "Farms",
                        principalColumn: "FarmsID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmsProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmsProducts_ProductID",
                table: "FarmsProducts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreID",
                table: "Products",
                column: "StoreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmsProducts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Clients",
                newName: "CategoryID");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreID = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryNotes = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CategoryUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Categories_Stores_StoreID",
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
                name: "IX_Categories_StoreID",
                table: "Categories",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmsCategories_CategoryID",
                table: "FarmsCategories",
                column: "CategoryID");
        }
    }
}
