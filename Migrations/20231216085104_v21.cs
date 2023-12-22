using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Financial",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Financial",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Financial");
        }
    }
}
