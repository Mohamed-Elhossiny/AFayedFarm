using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPercentage",
                table: "ExpenseRecords");

            migrationBuilder.AddColumn<bool>(
                name: "isPercentage",
                table: "FarmsProducts",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPercentage",
                table: "FarmsProducts");

            migrationBuilder.AddColumn<bool>(
                name: "isPercentage",
                table: "ExpenseRecords",
                type: "bit",
                nullable: true);
        }
    }
}
