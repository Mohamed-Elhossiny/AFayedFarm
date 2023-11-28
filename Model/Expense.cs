using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Expense
	{
		[Key]
		public int ExpenseID { get; set; }

		[ForeignKey("ExpenseType")]
		public int ExpenseTypeId { get; set; }
		public virtual TypeOfExpense? ExpenseType { get; set; }
		[DataType(DataType.Date)]
		[Column(TypeName = "Date")]
		public DateTime ExpenseDate { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Price { get; set; }
		public decimal? AdditionalQuantity { get; set; }
		public decimal? AdditionalPrice { get; set; }
		public decimal? TotalPrice { get; set; }
		public decimal? Paied { get; set; }
		public decimal? Remaining { get; set; }
		[Column(TypeName = "nvarchar(MAX)")]
		public string? ExpenseNotes { get; set; }
		[ForeignKey("Store")]
		public int? StoreID { get; set; }
		public virtual Store? Store { get; set; }
	}
}
