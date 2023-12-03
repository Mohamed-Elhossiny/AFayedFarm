using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos
{
	public class AddFarmDto
	{
		[Required(ErrorMessage = "Please Enter Farm Name")]
        public string Name { get; set; }
    }
}
