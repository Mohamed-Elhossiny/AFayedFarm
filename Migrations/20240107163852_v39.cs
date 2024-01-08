using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionProduct_Products_ProductID",
                table: "TransactionProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionProduct_Transactions_TransactionID",
                table: "TransactionProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionProduct",
                table: "TransactionProduct");

            migrationBuilder.RenameTable(
                name: "TransactionProduct",
                newName: "TransactionProducts");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionProduct_TransactionID",
                table: "TransactionProducts",
                newName: "IX_TransactionProducts_TransactionID");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionProduct_ProductID",
                table: "TransactionProducts",
                newName: "IX_TransactionProducts_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionProducts",
                table: "TransactionProducts",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionProducts_Products_ProductID",
                table: "TransactionProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionProducts_Transactions_TransactionID",
                table: "TransactionProducts",
                column: "TransactionID",
                principalTable: "Transactions",
                principalColumn: "TransactionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionProducts_Products_ProductID",
                table: "TransactionProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionProducts_Transactions_TransactionID",
                table: "TransactionProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionProducts",
                table: "TransactionProducts");

            migrationBuilder.RenameTable(
                name: "TransactionProducts",
                newName: "TransactionProduct");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionProducts_TransactionID",
                table: "TransactionProduct",
                newName: "IX_TransactionProduct_TransactionID");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionProducts_ProductID",
                table: "TransactionProduct",
                newName: "IX_TransactionProduct_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionProduct",
                table: "TransactionProduct",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionProduct_Products_ProductID",
                table: "TransactionProduct",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionProduct_Transactions_TransactionID",
                table: "TransactionProduct",
                column: "TransactionID",
                principalTable: "Transactions",
                principalColumn: "TransactionID");
        }
    }
}
