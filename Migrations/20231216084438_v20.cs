using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Financial",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionID = table.Column<int>(type: "int", nullable: true),
                    CLientID = table.Column<int>(type: "int", nullable: true),
                    TotalPerTrasaction = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financial", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Financial_Clients_CLientID",
                        column: x => x.CLientID,
                        principalTable: "Clients",
                        principalColumn: "ClientID");
                    table.ForeignKey(
                        name: "FK_Financial_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Financial_CLientID",
                table: "Financial",
                column: "CLientID");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_TransactionID",
                table: "Financial",
                column: "TransactionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Financial");
        }
    }
}
