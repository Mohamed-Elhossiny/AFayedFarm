using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreID",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_StoreID",
                table: "Expenses",
                column: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Stores_StoreID",
                table: "Expenses",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "StoreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Stores_StoreID",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_StoreID",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "StoreID",
                table: "Expenses");
        }
    }
}
