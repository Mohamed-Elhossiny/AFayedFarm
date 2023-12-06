using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CarNumber",
                table: "FarmsProducts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FarmRecordID",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_FarmRecordID",
                table: "Expenses",
                column: "FarmRecordID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_FarmsProducts_FarmRecordID",
                table: "Expenses",
                column: "FarmRecordID",
                principalTable: "FarmsProducts",
                principalColumn: "FarmProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_FarmsProducts_FarmRecordID",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_FarmRecordID",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "FarmRecordID",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "CarNumber",
                table: "FarmsProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
