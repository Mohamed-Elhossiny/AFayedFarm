using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Supplier
	{
		[Key]
		public int SupplierID { get; set; }
        public string? SupplierName { get; set; }

		[DataType(DataType.Date)]
		[Column(TypeName = "Date")]
		public DateTime? SupplyDate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal? NetQuantity { get; set; }
        public virtual ICollection<SupplyCategory>? SupplyCategory { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Paied { get; set; }
        public decimal? Remaining { get; set; }
		[Column(TypeName = "nvarchar(MAX)")]
		public string? SupplierNotes { get; set; }
        [ForeignKey("Store")]
        public int? StoreID { get; set; }
        public virtual Store? Store { get; set; }
    }
}
