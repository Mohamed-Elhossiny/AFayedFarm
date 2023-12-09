using AFayedFarm.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos
{
	public class AddFarmRecordDto
	{
		public AddFarmRecordDto()
		{
			Created_Date = DateTime.Now;
		}
		public int FarmsID { get; set; }
		public int ProductID { get; set; }
        public string? CarNumber { get; set; }
        public DateTime? SupplyDate { get; set; }
		public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Discount { get; set; }
		public decimal? NetQuantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
		public decimal? Paied { get; set; }
		public string? FarmsNotes { get; set; }
		public bool? isPercentage { get; set; }
	}
}
