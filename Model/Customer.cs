using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Model
{
	public class Customer
	{
		[Key]
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }

        /// <summary>
        /// 1 ==> Export OutSide
        /// 0 ==> Inside
        /// </summary>
        public int? Export { get; set; }
        [DataType(DataType.Date)]
        public DateTime ShippingDate { get; set; }
        public int? CategoryID { get; set; }
        public decimal? GetPaied { get; set; }
        public decimal? Remaining { get; set; }
    }
}
