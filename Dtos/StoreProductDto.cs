namespace AFayedFarm.Dtos
{
	public class StoreProductDto
	{
        public int? ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
