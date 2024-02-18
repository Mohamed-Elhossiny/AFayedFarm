namespace AFayedFarm.Dtos.Financial
{
	public class FinancialFarmDto : BaseFinancialRecordDto
	{
		//public int? index { get; set; }
		public int? ID { get; set; }
		public int? FarmID{ get; set; }
		public string? FarmName { get; set; }
	}
}
