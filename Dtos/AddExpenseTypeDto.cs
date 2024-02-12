using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos
{
	public class AddExpenseTypeDto
	{
		[MaxLength(50, ErrorMessage = "Max Lenght is 50")]
		[Required(ErrorMessage ="Please enter Expense Type")]
        public string ExpenseTypeName { get; set; }
    }
}
