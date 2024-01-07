using AFayedFarm.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Dtos
{
    public class AddFridgeRecordDto
    {
        public int? FridgeID { get; set; }
        public int? ProductID { get; set; }
        public int? Action { get; set; }
        public DateTime? SupplyDate { get; set; }
        public decimal? Number { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public decimal? Payed { get; set; }
        //public decimal? Remaining { get; set; }
		public int? TypeId { get; set; }
		public string? Notes { get; set; }
        public string? CarNumber { get; set; }
    }
}
