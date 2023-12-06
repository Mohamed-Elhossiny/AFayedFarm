namespace AFayedFarm.Dtos
{
	public class FarmRecordsWithTotalRemainingDto
	{
        public List<FarmRecordDto>? FarmRecords { get; set; } = new List<FarmRecordDto>();
        public decimal? TotalRemaining { get; set; }
    }
}
