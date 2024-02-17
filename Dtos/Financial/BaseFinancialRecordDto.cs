namespace AFayedFarm.Dtos.Financial
{
	public class BaseFinancialRecordDto
	{
		public int? TypeID { get; set; }
		public string? Type { get; set; }
		public string? Notes { get; set; }
		public DateTime? Date { get; set; }
		public int? SafeID { get; set; }
		public decimal? Total { get; set; }

	}
}
