using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v38 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Products_ProductID",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Stores_StoreID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ProductID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Transactions",
                newName: "TotalCapcity");

            migrationBuilder.RenameColumn(
                name: "GetPaied",
                table: "Transactions",
                newName: "Price");

            migrationBuilder.AlterColumn<int>(
                name: "StoreID",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Payed",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionProduct",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Qunatity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Number = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionProduct", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_ProductID",
                table: "TransactionProduct",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_TransactionID",
                table: "TransactionProduct",
                column: "TransactionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Stores_StoreID",
                table: "Transactions",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "StoreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Stores_StoreID",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionProduct");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Payed",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TotalCapcity",
                table: "Transactions",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Transactions",
                newName: "GetPaied");

            migrationBuilder.AlterColumn<int>(
                name: "StoreID",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ProductID",
                table: "Transactions",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Products_ProductID",
                table: "Transactions",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Stores_StoreID",
                table: "Transactions",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "StoreID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
