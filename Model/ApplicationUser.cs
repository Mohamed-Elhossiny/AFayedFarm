using Microsoft.AspNetCore.Identity;

namespace AFayedFarm.Model
{
	public class ApplicationUser : IdentityUser
	{
        public string? FName { get; set; }
    }
}
