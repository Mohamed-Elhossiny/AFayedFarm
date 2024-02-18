namespace AFayedFarm.Dtos
{
	public class FarmRecordsWithFarmDataDto
	{
		public List<FarmRecordDto>? FarmRecords { get; set; } = new List<FarmRecordDto>();
		public int? ID { get; set; }
		public string? Name { get; set; }
		public decimal? Total { get; set; }
        public DateTime? Date { get; set; }
    }
}
