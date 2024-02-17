namespace AFayedFarm.Dtos.Financial
{
	public class FinancialClientDto :BaseFinancialRecordDto
	{
		public int? ID { get; set; }
		public int? ClientID{ get; set; }
		public string? ClientName { get; set; }
	}
}
