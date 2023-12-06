using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Transaction
	{
		[Key]
		public int TransactionID { get; set; }
		
		[ForeignKey("Store")]
		public int StoreID { get; set; }

		[ForeignKey("Client")]
		public int ClientID { get; set; }

		[ForeignKey("Product")]
		public int ProductID { get; set; }
		public virtual Store? Store { get; set; }
		public virtual Client? Client { get; set; }
		public virtual Product? Product { get; set; }

		[DataType(DataType.Date)]
		public DateTime ShippingDate { get; set; }

		[DataType(DataType.Date)]
		[Column(TypeName = "Date")]
		public DateTime? Created_Date { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Total { get; set; }
		public decimal? GetPaied { get; set; }
		public decimal? Remaining { get; set; }
        public string? Notes { get; set; }
    }
}
