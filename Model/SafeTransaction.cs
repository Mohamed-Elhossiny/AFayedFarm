using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class SafeTransaction
	{
		[Key]
        public int ID { get; set; }
        
        [ForeignKey("Client")]
        public int? CLientID { get; set; }
        public virtual Client? Client { get; set; }

        [ForeignKey("Employee")]
        public int? Emp_ID { get; set; }
        public virtual Employee? Employee { get; set; }

		[ForeignKey("Farm")]
		public int? FarmID { get; set; }
        public virtual Farms? Farm { get; set; }

		[ForeignKey("Expense")]
		public int? ExpenseID { get; set; }
		public virtual Expense? Expense { get; set; }
		/// <summary>
		/// Total Per Transaction == Amount
		/// </summary>
		public decimal? Total { get; set; }
        public string? Notes { get; set; }
        public int? TypeID { get; set; }
        public string? Type { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? Created_Date { get; set; } = DateTime.Now.Date;
    }
}
