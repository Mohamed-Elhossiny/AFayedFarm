using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafeTransactions_Safe_FinancialSafeID",
                table: "SafeTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SafeTransactions_FinancialSafeID",
                table: "SafeTransactions");

            migrationBuilder.DropColumn(
                name: "FinancialSafeID",
                table: "SafeTransactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinancialSafeID",
                table: "SafeTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SafeTransactions_FinancialSafeID",
                table: "SafeTransactions",
                column: "FinancialSafeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SafeTransactions_Safe_FinancialSafeID",
                table: "SafeTransactions",
                column: "FinancialSafeID",
                principalTable: "Safe",
                principalColumn: "ID");
        }
    }
}
