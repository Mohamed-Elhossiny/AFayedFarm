namespace AFayedFarm.Dtos.Client
{
	public class AddProductListDto
	{
        public int? ProductID { get; set; }
        public decimal? Quantity { get; set; }
        public int? ProductBoxID { get; set; }
        public decimal? Number { get; set; }
        public decimal? ProductTotal { get; set; }
        public decimal? Price { get; set; }
    }
}
