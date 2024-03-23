namespace AFayedFarm.Dtos
{
	public class ExpenseDto
	{
        public int ID { get; set; }
        public string? Name { get; set; }
        public int? Type { get; set; }
        public string? ExpenseTypeName { get; set; }
        public decimal? TotalRemaining { get; set; }
		public List<ExpenseRecordDto>? OfflineRecords { get; set; } = new List<ExpenseRecordDto>();
	}
}
