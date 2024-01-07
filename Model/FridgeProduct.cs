using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Model
{
	public class FridgeProduct
	{
		[Key]
		public int FridgeProductID { get; set; }

		[ForeignKey("Product")]
		public int? ProductID { get; set; }

		[ForeignKey("Fridge")]
		public int? FridgeID { get; set; }

		[ForeignKey("Store")]
		public int? StoreID { get; set; }
		public virtual Product? Product { get; set; }
		public virtual Store? Store { get; set; }
		public virtual Fridge? Fridge { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? SupplyDate { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? TotalPrice { get; set; }
		public string? Notes { get; set; }
	}
}
