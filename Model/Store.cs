using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Model
{
	public class Store
	{
		[Key]
        public int StoreID { get; set; }
        public virtual ICollection<Farms>? Farms { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<Client>? Clients { get; set; }
        public virtual ICollection<Expense>? Expenses { get; set; }
    }
}
