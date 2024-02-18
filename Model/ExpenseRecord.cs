using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class ExpenseRecord
	{
		[Key]
        public int ExpenseRecordId { get; set; }

		[ForeignKey("FarmRecord")]
		public int? FarmRecordID { get; set; }
		public virtual FarmsProduct? FarmRecord { get; set; }

		[ForeignKey("FridgeProduct")]
		public int? FridgeProductID { get; set; }
		public virtual FridgeRecord? FridgeProduct { get; set; }

		[ForeignKey("Expense")]
        public int? ExpenseID { get; set; }
        public virtual Expense? Expense { get; set; }

		//[DataType(DataType.Date)]
		[Column(TypeName = "DateTime")]
		public DateTime? ExpenseDate { get; set; }

		//[DataType(DataType.Date)]
		[Column(TypeName = "DateTime")]
		public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Value { get; set; }
		public decimal? Price { get; set; }
		public decimal? AdditionalPrice { get; set; }
		public string? AdditionalNotes { get; set; }
		public decimal? Total { get; set; }
		public decimal? Paied { get; set; }
		public decimal? Remaining { get; set; }
		[Column(TypeName = "nvarchar(MAX)")]
		public string? ExpenseNotes { get; set; }
		[ForeignKey("Store")]
		public int? StoreID { get; set; }
		public virtual Store? Store { get; set; }
		public int? FinancialId { get; set; }
	}
}
