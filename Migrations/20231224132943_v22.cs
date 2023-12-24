using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AFayedFarm.Migrations
{
    /// <inheritdoc />
    public partial class v22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Financial");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpolyeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Full_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HireDate = table.Column<DateTime>(type: "Date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpolyeeID);
                });

            migrationBuilder.CreateTable(
                name: "Safe",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Safe", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTransactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmpID = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "Date", nullable: true),
                    TransactionTypeID = table.Column<int>(type: "int", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTransactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_EmployeeTransactions_Employees_EmpID",
                        column: x => x.EmpID,
                        principalTable: "Employees",
                        principalColumn: "EmpolyeeID");
                });

            migrationBuilder.CreateTable(
                name: "SafeTransactions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionID = table.Column<int>(type: "int", nullable: true),
                    CLientID = table.Column<int>(type: "int", nullable: true),
                    Emp_ID = table.Column<int>(type: "int", nullable: true),
                    TotalPerTrasaction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionTypeID = table.Column<int>(type: "int", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "Date", nullable: true),
                    FinancialSafeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeTransactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SafeTransactions_Clients_CLientID",
                        column: x => x.CLientID,
                        principalTable: "Clients",
                        principalColumn: "ClientID");
                    table.ForeignKey(
                        name: "FK_SafeTransactions_Employees_Emp_ID",
                        column: x => x.Emp_ID,
                        principalTable: "Employees",
                        principalColumn: "EmpolyeeID");
                    table.ForeignKey(
                        name: "FK_SafeTransactions_Safe_FinancialSafeID",
                        column: x => x.FinancialSafeID,
                        principalTable: "Safe",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SafeTransactions_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTransactions_EmpID",
                table: "EmployeeTransactions",
                column: "EmpID");

            migrationBuilder.CreateIndex(
                name: "IX_SafeTransactions_CLientID",
                table: "SafeTransactions",
                column: "CLientID");

            migrationBuilder.CreateIndex(
                name: "IX_SafeTransactions_Emp_ID",
                table: "SafeTransactions",
                column: "Emp_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SafeTransactions_FinancialSafeID",
                table: "SafeTransactions",
                column: "FinancialSafeID");

            migrationBuilder.CreateIndex(
                name: "IX_SafeTransactions_TransactionID",
                table: "SafeTransactions",
                column: "TransactionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTransactions");

            migrationBuilder.DropTable(
                name: "SafeTransactions");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Safe");

            migrationBuilder.CreateTable(
                name: "Financial",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLientID = table.Column<int>(type: "int", nullable: true),
                    TransactionID = table.Column<int>(type: "int", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "Date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
    }
}
