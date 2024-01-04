using AFayedFarm.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AFayedFarm.Dtos
{
	public class ExpenseRecordDto
	{
		public int? ExpenseRecordID { get; set; }
        public int? ExpenseID { get; set; }
        public string? ProductName { get; set; }
		public string? Description { get; set; }
		public string? ExpenseName { get; set; }
		public string? ExpenseTypeName { get; set; }
		public int? FarmRecordID { get; set; }
		public DateTime? ExpenseDate { get; set; }
		public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? Value { get; set; }
        public decimal? Price { get; set; }
        public decimal? AdditionalPrice { get; set; }
		public string? AdditionalNotes { get; set; }
		public decimal? Total { get; set; }
		public decimal? Paied { get; set; }
        public int? TypeId { get; set; }
        public decimal? Remaining { get; set; }
		public string? ExpenseRecordNotes { get; set; }

	}
}
