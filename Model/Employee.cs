using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Employee
	{
		[Key]
		public int EmpolyeeID { get; set; }
		public string? Full_Name { get; set; }
		public decimal? Salary { get; set; }
		public decimal? TotalBalance { get; set; }

		[Column(TypeName = "DateTime")]
		public DateTime? HireDate { get; set; }

		[Column(TypeName = "DateTime")]
		public DateTime? Create_Date { get; set; }
		public string? Address { get; set; }
		public string? Phone { get; set; }
		public virtual ICollection<SafeTransaction>? Transactions { get; set; }
	}
}
