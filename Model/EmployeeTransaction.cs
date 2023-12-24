using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class EmployeeTransaction
	{
		[Key]
		public int TransactionID { get; set; }
		public decimal? Amount { get; set; }

		[ForeignKey("Employee")]
		public int? EmpID { get; set; }
		public virtual Employee? Employee { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? Date { get; set; }
		public int? TransactionTypeID { get; set; }
		public string? TransactionType { get; set; }
		public string? Notes { get; set; }
	}
}
