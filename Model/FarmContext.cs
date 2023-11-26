using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Model
{
	public class FarmContext : IdentityDbContext<ApplicationUser>
	{
		public FarmContext() { }
        public FarmContext(DbContextOptions<FarmContext> options): base(options) { }
        
    }
}
