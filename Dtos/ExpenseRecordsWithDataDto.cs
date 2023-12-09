namespace AFayedFarm.Dtos
{
	public class ExpenseRecordsWithDataDto
	{
		public List<ExpenseRecordDto>? ExpensesList { get; set; } = new List<ExpenseRecordDto>();
        public int? ID { get; set; }
		public string? Name { get; set; }
		public decimal? Total { get; set; }
	}
}
