using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Product
	{
		[Key]
		public int ProductID { get; set; }
		public string? ProductName { get; set; }
		public decimal? ProductUnitPrice { get; set; }

		[Column(TypeName = "nvarchar(MAX)")]
		public string? ProductNote { get; set; }

		//[DataType(DataType.Date)]
		[Column(TypeName = "DateTime")]
		public DateTime? Created_Date { get; set; }
        public virtual ICollection<FarmsProduct>? FarmsProducts { get; set; }
        public virtual ICollection<FridgeRecord>? FridgeProducts { get; set; }
        public virtual ICollection<StoreProduct>? StoreProducts { get; set; }
		//public virtual ICollection<Transaction>? Transactions { get; set; }
		public virtual ICollection<TransactionProduct>? TransactionProducts { get; set; }

	}
}
