using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_FarmsProducts_FarmRecordID",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_FarmRecordID",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "AdditionalPrice",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "AdditionalQuantity",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseDate",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseNotes",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "FarmRecordID",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Paied",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Remaining",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Expenses");

            migrationBuilder.CreateTable(
                name: "ExpenseRecords",
                columns: table => new
                {
                    ExpenseRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmRecordID = table.Column<int>(type: "int", nullable: true),
                    ExpenseID = table.Column<int>(type: "int", nullable: true),
                    ExpenseDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "Date", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Paied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpenseNotes = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    StoreID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseRecords", x => x.ExpenseRecordId);
                    table.ForeignKey(
                        name: "FK_ExpenseRecords_Expenses_ExpenseID",
                        column: x => x.ExpenseID,
                        principalTable: "Expenses",
                        principalColumn: "ExpenseID");
                    table.ForeignKey(
                        name: "FK_ExpenseRecords_FarmsProducts_FarmRecordID",
                        column: x => x.FarmRecordID,
                        principalTable: "FarmsProducts",
                        principalColumn: "FarmProductID");
                    table.ForeignKey(
                        name: "FK_ExpenseRecords_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseRecords_ExpenseID",
                table: "ExpenseRecords",
                column: "ExpenseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseRecords_FarmRecordID",
                table: "ExpenseRecords",
                column: "FarmRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseRecords_StoreID",
                table: "ExpenseRecords",
                column: "StoreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseRecords");

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalPrice",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalQuantity",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Expenses",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpenseDate",
                table: "Expenses",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ExpenseNotes",
                table: "Expenses",
                type: "nvarchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FarmRecordID",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Paied",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Remaining",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Expenses",
                type: "decimal(18,2)",
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
    }
}
