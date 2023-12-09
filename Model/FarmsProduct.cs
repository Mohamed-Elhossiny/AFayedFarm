using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class FarmsProduct
	{
		[Key]
        public int FarmProductID { get; set; }

        [ForeignKey("Farms")]
        public int? FarmsID { get; set; }
        [ForeignKey("Product")]
        public int? ProductID { get; set; }
        public virtual Farms? Farms { get; set; }
        public virtual Product? Product { get; set; }
		
		[Column(TypeName = "Date")]
		public DateTime? SupplyDate { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Discount { get; set; }
		public bool? isPercentage { get; set; }
		public decimal? NetQuantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
		public decimal? Paied { get; set; }
		public decimal? Remaining { get; set; }
		[Column(TypeName = "nvarchar(MAX)")]
		public string? FarmsNotes { get; set; }
        public string? CarNumber { get; set; }
        public virtual ICollection<ExpenseRecord>? ExpeneseRecordList { get; set; }
    }
}
