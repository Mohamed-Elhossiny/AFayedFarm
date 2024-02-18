namespace AFayedFarm.Dtos.Financial
{
	public class FinancialExpenseDto : BaseFinancialRecordDto
	{
		public int? index { get; set; }
		public int? ID { get; set; }
		public int? ExpenseID { get; set; }
		public string? ExpenseName { get; set; }
	}
}
