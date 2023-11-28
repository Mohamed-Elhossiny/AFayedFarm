using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Model
{
	public class Store
	{
		[Key]
        public int StoreID { get; set; }
        public virtual ICollection<Supplier>? Suppliers { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<Customer>? Customers { get; set; }
        public virtual ICollection<Expense>? Expenses { get; set; }
    }
}
