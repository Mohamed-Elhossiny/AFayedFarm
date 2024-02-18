using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Farms
	{
		[Key]
		public int FarmsID { get; set; }
        public string? FarmsName { get; set; }
		[Column(TypeName = "DateTime")]
		public DateTime? Create_Date { get; set; }
        public decimal? TotalRemaining { get; set; }
        public virtual ICollection<FarmsProduct>? FarmsProducts { get; set; }
		public virtual ICollection<SafeTransaction>? Transactions { get; set; }

	}
}
