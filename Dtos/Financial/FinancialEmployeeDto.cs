namespace AFayedFarm.Dtos.Financial
{
	public class FinancialEmployeeDto : BaseFinancialRecordDto
	{
		public int? index { get; set; }
		public int? ID { get; set; }
		public int? EmpID { get; set; }
        public string? EmpName { get; set; }
    }
}
