namespace AFayedFarm.Dtos.Client
{
	public class AddProductListDto
	{
		public int? Id { get; set; }
		public int? ProductID { get; set; }
		public decimal? Quantity { get; set; }
		public int? ProductBoxID { get; set; }
		public decimal? Number { get; set; }
		public decimal? Price { get; set; }
		public decimal? Total { get; set; }
		public int? StatusID { get; set; } = 1; //Current == 1
        //public decimal? ProductTotal { get; set; }
    }
}
