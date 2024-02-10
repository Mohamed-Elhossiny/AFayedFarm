namespace AFayedFarm.Dtos
{
	public class UpdateStoreProductDto
	{
        public int? StoreId{ get; set; }
        public int? ProductID { get; set; }
        public decimal? Quantity { get; set; }

		public int? ProductBoxID { get; set; }
		public decimal? Number { get; set; }
	}
}
