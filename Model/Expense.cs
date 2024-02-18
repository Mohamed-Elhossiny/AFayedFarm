using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Expense
	{
		[Key]
		public int ExpenseID { get; set; }
		public string? ExpenseName { get; set; }
		public decimal? TotalRemaining { get; set; } = default(decimal?);

		[Column(TypeName = "DateTime")]
		public DateTime? Create_Date { get; set; }

		[ForeignKey("ExpenseType")]
		public int ExpenseTypeId { get; set; }
		public virtual TypeOfExpense? ExpenseType { get; set; }
        public virtual ICollection<ExpenseRecord>? ExpenseRecords { get; set; }
		public virtual ICollection<SafeTransaction>? Transactions { get; set; }

	}
}
