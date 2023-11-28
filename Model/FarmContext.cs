using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Model
{
	public class FarmContext : IdentityDbContext<ApplicationUser>
	{
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Farms> Farms { get; set; }
        public virtual DbSet<FarmsCategory> FarmsCategories { get; set; }
        public virtual DbSet<TypeOfExpense> TypeOfExpenses { get; set; }
        public FarmContext() { }
        public FarmContext(DbContextOptions<FarmContext> options): base(options) { }
        
    }
}
