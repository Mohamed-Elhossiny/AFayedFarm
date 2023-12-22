using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Financial
	{
		[Key]
        public int ID { get; set; }
        
        [ForeignKey("Transaction")]
        public int? TransactionID { get; set; }

        [ForeignKey("Client")]
        public int? CLientID { get; set; }
        public virtual Transaction? Transaction { get; set; }
        public virtual Client? Client { get; set; }

        public decimal? TotalPerTrasaction { get; set; }
        public string? Notes { get; set; }

        [DataType(DataType.Date)]
		[Column(TypeName = "Date")]
		public DateTime? Created_Date { get; set; }
    }
}
