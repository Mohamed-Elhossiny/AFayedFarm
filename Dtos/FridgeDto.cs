﻿namespace AFayedFarm.Dtos
{
	public class FridgeDto
	{
		public int? ID { get; set; }
		public string? Name { get; set; }
		public decimal? Total { get; set; }
		public DateTime? Created_Date { get; set; }
		public List<FridgeRecordDto>? OfflineRecords { get; set; } = new List<FridgeRecordDto>();
	}
}
