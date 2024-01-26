using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Transaction
	{
		[Key]
		public int TransactionID { get; set; }

		[ForeignKey("Client")]
		public int ClientID { get; set; }
		public virtual Client? Client { get; set; }

        public virtual ICollection<TransactionProduct>? TransactionProducts { get; set; }

        [Column(TypeName = "Date")]
		public DateTime ShippingDate { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? Created_Date { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? PayDate { get; set; }
		public string? DriverName { get; set; }
		public decimal? TotalCapcity { get; set; }
		public decimal? DeliveredToDriver { get; set; }
		public decimal? Price { get; set; }
		public decimal? Total { get; set; }
		public decimal? Payed { get; set; }
		public decimal? Remaining { get; set; }
		public string? Notes { get; set; }
	}
}
