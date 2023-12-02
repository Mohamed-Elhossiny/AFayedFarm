using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Farms
	{
		[Key]
		public int FarmsID { get; set; }
        public string? FarmsName { get; set; }
        public virtual ICollection<FarmsProduct>? FarmsProducts { get; set; }

    }
}
