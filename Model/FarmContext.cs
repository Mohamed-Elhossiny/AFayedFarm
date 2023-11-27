using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Model
{
	public class FarmContext : IdentityDbContext<ApplicationUser>
	{
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplyCategory> SupplyCategories { get; set; }
        public virtual DbSet<TypeOfExpense> TypeOfExpenses { get; set; }
        public FarmContext() { }
        public FarmContext(DbContextOptions<FarmContext> options): base(options) { }
        
    }
}
