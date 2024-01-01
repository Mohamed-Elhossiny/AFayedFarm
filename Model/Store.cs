using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Store
	{
		[Key]
        public int StoreID { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? Created_Date { get; set; }
		public virtual ICollection<StoreProduct>? StoreProducts { get; set; }
		public virtual ICollection<Transaction>? Transactions { get; set; }
		public virtual ICollection<Expense>? Expenses { get; set; }

	}
}
