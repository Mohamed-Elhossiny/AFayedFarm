using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("Store")]
        public int? StoreID{ get; set; }
        public virtual Store? Store { get; set; }
    }
}
