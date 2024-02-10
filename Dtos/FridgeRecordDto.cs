namespace AFayedFarm.Dtos
{
	public class FridgeRecordDto
	{
		public int? Id { get; set; }
		public int? FridgeRecordID { get; set; }
		public int? FridgeID { get; set; }
		public string? FridgeName { get; set; }
		public int? ProductID { get; set; }
		public string? ProductName { get; set; }
		public string? Description { get; set; }
		public DateTime? SupplyDate { get; set; }
		public DateTime? Created_Date { get; set; }
		public decimal? Number { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Price { get; set; }
		public decimal? Total { get; set; }
		public decimal? Payed { get; set; }
		public decimal? Remaining { get; set; }
		public string? Notes { get; set; }
		public string? CarNumber { get; set; }
		public int? Action { get; set; }
		public string? ActionName { get; set; }
	}
}
