using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdditionalQuantity",
                table: "ExpenseRecords",
                newName: "Value");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "ExpenseRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "ExpenseRecords");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ExpenseRecords",
                newName: "AdditionalQuantity");
        }
    }
}
