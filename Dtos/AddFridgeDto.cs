using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos
{
	public class AddFridgeDto
	{
		[Required(ErrorMessage = "Please Enter Fridge Name")]
		public string? Name { get; set; }
	}
}
