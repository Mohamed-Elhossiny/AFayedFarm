using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos
{
	public class AddExpenseTypeDto
	{
		[Required(ErrorMessage ="Please enter Expense Type")]
        public string ExpenseTypeName { get; set; }
    }
}
