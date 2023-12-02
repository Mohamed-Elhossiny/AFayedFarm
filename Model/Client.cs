using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class Client
	{
		[Key]
        public int ClientID { get; set; }
        public string? ClientName { get; set; }

        /// <summary>
        /// 1 ==> Export OutSide
        /// 0 ==> Inside
        /// </summary>
        public int? Export { get; set; }
		public virtual ICollection<Transaction>? Transactions { get; set; }
	}
}
