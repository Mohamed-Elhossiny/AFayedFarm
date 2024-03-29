﻿namespace AFayedFarm.Dtos
{
	public class FarmRecordDto
	{
		public int? Id { get; set; }
		public int? FarmRecordID { get; set; }
		public int FarmsID { get; set; }
		public string? FarmsName { get; set; }
		public int? ProductID { get; set; }
		public string? ProductName { get; set; }
		public string? Description { get; set; }
		public DateTime? SupplyDate { get; set; }
		public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Discount { get; set; }
		public bool? isPercentage { get; set; }
		public decimal? NetQuantity { get; set; }
		public decimal? Price { get; set; }
		public decimal? Total { get; set; }
		public decimal? Paied { get; set; }
		public int? TypeId { get; set; }
		public decimal? Remaining { get; set; }
		public string? FarmsNotes { get; set; }
		public string? CarNumber { get; set; }
	}
}
