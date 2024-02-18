namespace AFayedFarm.Dtos.Financial
{
	public class AllFinancialRecordsDto : BaseFinancialRecordDto
	{
        public int? index { get; set; }
        public int? ID { get; set; }
		public int? ClientID { get; set; }
		public string? ClientName { get; set; }
		public int? EmpID { get; set; }
		public string? EmpName { get; set; }
		public int? ExpenseID { get; set; }
		public string? ExpenseName { get; set; }
		public int? FarmID { get; set; }
		public string? FarmName { get; set; }
		public int? FridgeID { get; set; }
		public string? FridgeName { get; set; }
	}
}
