namespace AFayedFarm.Dtos
{
	public class AddExpenseRecordDto
	{
        public int? ExpenseID { get; set; }
		public int? FarmRecordID { get; set; }
		public string? ExpenseName { get; set; }
		public DateTime? ExpenseDate { get; set; }
		//public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Value { get; set; }
		public decimal? Price { get; set; }
		public decimal? AdditionalPrice { get; set; }
		public string? AdditionalNotes { get; set; }
		public decimal? Total { get; set; }
		public decimal? Paied { get; set; }
		public decimal? Remaining { get; set; }
		public string? ExpenseRecordNotes { get; set; }
	}
}
