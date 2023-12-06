using AFayedFarm.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos
{
	public class AddTransactionDto
	{
		public int? StoreID { get; set; }
		public int? ClientID { get; set; }
		public int? ProductID { get; set; }
        public string? ProductName { get; set; }
		public DateTime? ShippingDate { get; set; }
		public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Total { get; set; }
		public decimal? GetPaied { get; set; }
        public string? Notes { get; set; }
    }
}
