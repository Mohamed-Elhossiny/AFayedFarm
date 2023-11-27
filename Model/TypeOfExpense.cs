using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Model
{
	public class TypeOfExpense
	{
		[Key]
        public int ExpenseTypeID { get; set; }
        public string? ExpenseTypeName { get; set; }
        public virtual ICollection<Expense>? Expense { get; set; }
    }
}
