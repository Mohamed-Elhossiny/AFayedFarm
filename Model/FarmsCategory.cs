using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
    [PrimaryKey(nameof(FarmsID),nameof(CategoryID))]
	public class FarmsCategory
	{
		[ForeignKey("Farms")]
        public int? FarmsID { get; set; }
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public virtual Farms? Farms { get; set; }
        public virtual Category? Category { get; set; }
    }
}
