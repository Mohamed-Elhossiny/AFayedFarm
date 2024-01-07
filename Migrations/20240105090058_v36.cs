using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FridgeID",
                table: "FridgeProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FridgeProducts_FridgeID",
                table: "FridgeProducts",
                column: "FridgeID");

            migrationBuilder.AddForeignKey(
                name: "FK_FridgeProducts_Fridges_FridgeID",
                table: "FridgeProducts",
                column: "FridgeID",
                principalTable: "Fridges",
                principalColumn: "FridgeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FridgeProducts_Fridges_FridgeID",
                table: "FridgeProducts");

            migrationBuilder.DropIndex(
                name: "IX_FridgeProducts_FridgeID",
                table: "FridgeProducts");

            migrationBuilder.DropColumn(
                name: "FridgeID",
                table: "FridgeProducts");
        }
    }
}
