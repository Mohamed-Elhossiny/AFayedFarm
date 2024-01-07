namespace AFayedFarm.Dtos
{
	public class AllFridgeRecordsWithTotalDto
	{
		public List<FridgeRecordDto>? FridgeRecords { get; set; } = new List<FridgeRecordDto>();
		public decimal? TotalRemaining { get; set; }
	}
}
