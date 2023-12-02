﻿using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos
{
	public class AddExpenseDto
	{
		[Required(ErrorMessage ="Please Enter Expense Name")]
		public string ExpenseName { get; set; }
		public int ExpenseTypeId { get; set; }
	}
}
