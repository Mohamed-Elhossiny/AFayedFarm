namespace AFayedFarm.Dtos.Client
{
	public class AddTransactionMainDataDto
	{
		public int? ClientID { get; set; }
		public string? DriverName { get; set; }
		public DateTime? Date { get; set; }
		public int? TypeId { get; set; }
		public decimal? Total { get; set; }
		public decimal? Payed { get; set; }
		public decimal? CarCapacity { get; set; }
		public string? Notes { get; set; }
        public List<AddProductListDto>? ProductList { get; set; }

    }
}
