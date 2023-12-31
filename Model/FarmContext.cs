﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Model
{
	public class FarmContext : IdentityDbContext<ApplicationUser>
	{
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<Client> Clients { get; set; }
		public virtual DbSet<Expense> Expenses { get; set; }
		public virtual DbSet<Store> Stores { get; set; }
		public virtual DbSet<Farms> Farms { get; set; }
		public virtual DbSet<FarmsProduct> FarmsProducts { get; set; }
		public virtual DbSet<StoreProduct> StoreProducts { get; set; }
		public virtual DbSet<TypeOfExpense> TypeOfExpenses { get; set; }
		public virtual DbSet<Transaction> Transactions { get; set; }
		public virtual DbSet<ExpenseRecord> ExpenseRecords { get; set; }
		public virtual DbSet<SafeTransaction> SafeTransactions { get; set; }
		public virtual DbSet<Employee> Employees { get; set; }
		public virtual DbSet<FinancialSafe> Safe { get; set; }
		public virtual DbSet<FridgeRecord> FridgeRecords { get; set; }
		public virtual DbSet<Fridge> Fridges { get; set; }
		public virtual DbSet<FridgeProduct> FridgeProducts { get; set; }
		public virtual DbSet<TransactionProduct> TransactionProducts { get; set; }
		public FarmContext() { }
		public FarmContext(DbContextOptions<FarmContext> options) : base(options) { }

	}
}
