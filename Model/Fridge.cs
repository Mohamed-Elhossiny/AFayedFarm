using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Model
{
	public class Fridge
	{
		[Key]
		public int FridgeID { get; set; }
		public string? FridgeName { get; set; }
		[Column(TypeName = "DateTime")]
		public DateTime? Created_Date { get; set; }
		public decimal? TotalRemaining { get; set; }
		public virtual ICollection<FridgeRecord>? FridgeRecords { get; set; }
		public virtual ICollection<FridgeProduct>? FridgeProducts { get; set; }
		public virtual ICollection<SafeTransaction>? Transactions { get; set; }
	}
}
