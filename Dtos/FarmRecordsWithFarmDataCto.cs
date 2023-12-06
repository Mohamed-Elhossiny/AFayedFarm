namespace AFayedFarm.Dtos
{
	public class FarmRecordsWithFarmDataCto
	{
		public List<FarmRecordDto>? FarmRecords { get; set; } = new List<FarmRecordDto>();
		public int? ID { get; set; }
		public string? Name { get; set; }
		public decimal? Total { get; set; }
        public DateOnly? Date { get; set; }
    }
}
