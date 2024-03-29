﻿namespace AFayedFarm.Dtos
{
	public class FarmDto
	{
        public int? ID { get; set; }
        public string? Name { get; set; }
        public decimal? Total { get; set; }
        public DateTime? Created_Date { get; set; }
		public List<FarmRecordDto>? OfflineRecords { get; set; } = new List<FarmRecordDto>();
	}
}
