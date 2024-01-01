using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SafeID",
                table: "SafeTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SafeTransactions_SafeID",
                table: "SafeTransactions",
                column: "SafeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SafeTransactions_Safe_SafeID",
                table: "SafeTransactions",
                column: "SafeID",
                principalTable: "Safe",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafeTransactions_Safe_SafeID",
                table: "SafeTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SafeTransactions_SafeID",
                table: "SafeTransactions");

            migrationBuilder.DropColumn(
                name: "SafeID",
                table: "SafeTransactions");
        }
    }
}
