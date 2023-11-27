using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
    [PrimaryKey(nameof(SupplierID),nameof(CategoryID))]
	public class SupplyCategory
	{
		[ForeignKey("Supplier")]
        public int? SupplierID { get; set; }
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual Category? Category { get; set; }
    }
}
