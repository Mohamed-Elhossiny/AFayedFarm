using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Category
	{
		[Key]
		public int CategoryID { get; set; }
		public string? CategoryName { get; set; }
		public decimal? CategoryUnitPrice { get; set; }

		[Column(TypeName = "nvarchar(MAX)")]
		public string? CategoryNotes { get; set; }
		public virtual ICollection<FarmsCategory>? FarmsCategory { get; set; }
		[ForeignKey("Store")]
		public int? StoreID { get; set; }
		public virtual Store? Store { get; set; }
	}
}
