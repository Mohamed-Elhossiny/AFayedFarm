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

		[ForeignKey("Fridge")]
		public int? FridgeID { get; set; }
		public virtual Fridge? Fridge { get; set; }

		[ForeignKey("Expense")]
		public int? ExpenseID { get; set; }
		public virtual Expense? Expense { get; set; }

		[ForeignKey("Safe")]
		public int? SafeID { get; set; }
		public virtual FinancialSafe? Safe { get; set; }
		/// <summary>
		/// Total Per Transaction == Amount
		/// </summary>
		public decimal? Total { get; set; }
        public string? Notes { get; set; }
        public int? TypeID { get; set; }
        public string? Type { get; set; }

		[Column(TypeName = "DateTime")]
		public DateTime? Created_Date { get; set; } = DateTime.Now;
		public bool? IsfromRecord { get; set; }
	}
}
