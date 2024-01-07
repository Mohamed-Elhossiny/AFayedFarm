using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Model
{
	public class FridgeRecord
	{
		[Key]
		public int FridgeRecordID { get; set; }

		[ForeignKey("Fridge")]
		public int? FridgeID { get; set; }
		[ForeignKey("Product")]
		public int? ProductID { get; set; }
		public virtual Fridge? Fridge { get; set; }
		public virtual Product? Product { get; set; }
        public int? Action { get; set; }
        public string? ActionName { get; set; }

        [Column(TypeName = "Date")]
		public DateTime? SupplyDate { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? Created_Date { get; set; }
		public decimal? Number { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Price { get; set; }
		public decimal? Total { get; set; }
		public decimal? Payed { get; set; }
		public decimal? Remaining { get; set; }
		[Column(TypeName = "nvarchar(MAX)")]
		public string? FridgeNotes { get; set; }
		public string? CarNumber { get; set; }
		public virtual ICollection<ExpenseRecord>? ExpeneseRecordList { get; set; }
	}
}
