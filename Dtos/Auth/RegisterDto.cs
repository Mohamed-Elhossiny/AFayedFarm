using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos.Auth
{
	public class RegisterDto
	{
		[Required, StringLength(150)]
		public string Name { get; set; }

		[Required, StringLength(150)]
		public string UserName { get; set; }

		[Required, StringLength(150)]
		public string Email { get; set; }

		[Required, StringLength(225)]
		public string Password { get; set; }
	}
}
