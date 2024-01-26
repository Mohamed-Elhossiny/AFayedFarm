namespace AFayedFarm.Dtos.Client
{
	public class TransactionMainDataDto
	{
        public int? ID { get; set; }
        public int? ClientID { get; set; }
		public string? ClientName { get; set; }
		public string? DriverName { get; set; }
		public DateTime? Date { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? PayDate { get; set; }
        public int? TypeId { get; set; }
		public decimal? DeliveredToDriver { get; set; }
		public decimal? Total { get; set; }
		public decimal? Payed { get; set; }
		public decimal? Remaining { get; set; }
		public decimal? CarCapacity { get; set; }
		public string? Notes { get; set; }
		public List<ProductListDto>? ProductList { get; set; }
	}
}
