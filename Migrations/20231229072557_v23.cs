using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafeTransactions_Transactions_TransactionID",
                table: "SafeTransactions");

            migrationBuilder.RenameColumn(
                name: "TransactionTypeID",
                table: "SafeTransactions",
                newName: "TypeID");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "SafeTransactions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TransactionID",
                table: "SafeTransactions",
                newName: "FarmID");

            migrationBuilder.RenameColumn(
                name: "TotalPerTrasaction",
                table: "SafeTransactions",
                newName: "Total");

            migrationBuilder.RenameIndex(
                name: "IX_SafeTransactions_TransactionID",
                table: "SafeTransactions",
                newName: "IX_SafeTransactions_FarmID");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseID",
                table: "SafeTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SafeTransactions_ExpenseID",
                table: "SafeTransactions",
                column: "ExpenseID");

            migrationBuilder.AddForeignKey(
                name: "FK_SafeTransactions_Expenses_ExpenseID",
                table: "SafeTransactions",
                column: "ExpenseID",
                principalTable: "Expenses",
                principalColumn: "ExpenseID");

            migrationBuilder.AddForeignKey(
                name: "FK_SafeTransactions_Farms_FarmID",
                table: "SafeTransactions",
                column: "FarmID",
                principalTable: "Farms",
                principalColumn: "FarmsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafeTransactions_Expenses_ExpenseID",
                table: "SafeTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SafeTransactions_Farms_FarmID",
                table: "SafeTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SafeTransactions_ExpenseID",
                table: "SafeTransactions");

            migrationBuilder.DropColumn(
                name: "ExpenseID",
                table: "SafeTransactions");

            migrationBuilder.RenameColumn(
                name: "TypeID",
                table: "SafeTransactions",
                newName: "TransactionTypeID");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SafeTransactions",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "SafeTransactions",
                newName: "TotalPerTrasaction");

            migrationBuilder.RenameColumn(
                name: "FarmID",
                table: "SafeTransactions",
                newName: "TransactionID");

            migrationBuilder.RenameIndex(
                name: "IX_SafeTransactions_FarmID",
                table: "SafeTransactions",
                newName: "IX_SafeTransactions_TransactionID");

            migrationBuilder.AddForeignKey(
                name: "FK_SafeTransactions_Transactions_TransactionID",
                table: "SafeTransactions",
                column: "TransactionID",
                principalTable: "Transactions",
                principalColumn: "TransactionID");
        }
    }
}
