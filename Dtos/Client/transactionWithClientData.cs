namespace AFayedFarm.Dtos.Client
{
	public class TransactionWithClientData
	{
		public int? ID { get; set; }
		public string? Name { get; set; }
		public DateOnly? Date { get; set; }
        public decimal? Total { get; set; }
        public List<TransactionMainDataDto>? TransactionsList { get; set; }
	}
}
