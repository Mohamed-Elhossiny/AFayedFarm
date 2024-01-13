using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Client
	{
		[Key]
        public int ClientID { get; set; }
        public string? ClientName { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? Create_Date { get; set; }
        public decimal? Total { get; set; }

        /// <summary>
        /// 1 ==> Export OutSide
        /// 0 ==> Inside
        /// </summary>
        public int? Export { get; set; }
		public virtual ICollection<Transaction>? Transactions { get; set; }
		public virtual ICollection<SafeTransaction>? SafeTransactions { get; set; }
	}
}
