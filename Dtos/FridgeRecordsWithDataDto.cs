namespace AFayedFarm.Dtos
{
	public class FridgeRecordsWithDataDto
	{
		public List<FridgeRecordDto>? FridgeRecords { get; set; } = new List<FridgeRecordDto>();
		public int? ID { get; set; }
		public string? Name { get; set; }
		public decimal? Total { get; set; }
		public DateOnly? Date { get; set; }
	}
}
