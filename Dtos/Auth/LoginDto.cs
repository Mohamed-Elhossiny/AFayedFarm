using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos.Auth
{
	public class LoginDto
	{

		[Required, MaxLength(100)]
		public string Email { get; set; }

		[Required, MaxLength(250)]
		public string Password { get; set; }
	}
}
