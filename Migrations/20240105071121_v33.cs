using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FridgeProductID",
                table: "ExpenseRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseRecords_FridgeProductID",
                table: "ExpenseRecords",
                column: "FridgeProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseRecords_FridgeProducts_FridgeProductID",
                table: "ExpenseRecords",
                column: "FridgeProductID",
                principalTable: "FridgeProducts",
                principalColumn: "FridgeProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseRecords_FridgeProducts_FridgeProductID",
                table: "ExpenseRecords");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseRecords_FridgeProductID",
                table: "ExpenseRecords");

            migrationBuilder.DropColumn(
                name: "FridgeProductID",
                table: "ExpenseRecords");
        }
    }
}
