namespace AFayedFarm.Dtos
{
	public class TransactionDto
	{
        public int? TransactionID { get; set; }
        public int? StoreID { get; set; }
		public int? ClientID { get; set; }
        public string? ClientName { get; set; }
        public int? ProductID { get; set; }
		public string? ProductName { get; set; }
		public DateTime? ShippingDate { get; set; }
		public DateTime? Created_Date { get; set; }
		public decimal? Total { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? GetPaied { get; set; }
		public decimal? Remaining { get; set; }
		public string? Notes { get; set; }
	}
}
