using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippingDate",
                table: "Transactions",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "Create_Date",
                table: "Farms",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Create_Date",
                table: "Expenses",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Create_Date",
                table: "Clients",
                type: "Date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create_Date",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "Create_Date",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Create_Date",
                table: "Clients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippingDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");
        }
    }
}
