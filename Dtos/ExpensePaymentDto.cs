namespace AFayedFarm.Dtos
{
	public class ExpensePaymentDto
	{
		public int? Id { get; set; }
		public decimal? Total { get; set; }
		public int? TrasactionTypeID { get; set; }
		public string? Notes { get; set; }
	}
}
