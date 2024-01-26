namespace AFayedFarm.Dtos.Client
{
	public class ProductListDto
	{
        public int Id { get; set; }
        public int? ProductID { get; set; }
		public string? ProductName { get; set; }
		public decimal? Quantity { get; set; }
		public int? ProductBoxID { get; set; }
		public string? ProductBoxName { get; set; }
		public decimal? Number { get; set; }
		public decimal? Total { get; set; }
		public decimal? Price { get; set; }
	}
}
