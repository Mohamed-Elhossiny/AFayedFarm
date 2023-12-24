using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Model
{
	public class FinancialSafe
	{
		[Key]
        public int ID { get; set; }
        public decimal? Total { get; set; }
        public virtual ICollection<SafeTransaction>? Transactions { get; set; }
    }
}
