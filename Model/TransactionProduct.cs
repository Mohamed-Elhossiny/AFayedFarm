using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class TransactionProduct
	{
		[Key]
		public int ID { get; set; }

		[ForeignKey("Transaction")]
		public int? TransactionID { get; set; }
		public virtual Transaction? Transaction { get; set; }

		[ForeignKey("Product")]
		public int? ProductID { get; set; }
		public virtual Product? Product { get; set; }
		public decimal? Qunatity { get; set; }
		public decimal? Number { get; set; }
        public decimal? Price { get; set; }
        public decimal? ProductTotal { get; set; }
    }
}
