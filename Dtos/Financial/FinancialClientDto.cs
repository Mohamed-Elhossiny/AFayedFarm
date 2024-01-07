namespace AFayedFarm.Dtos.Financial
{
	public class FinancialClientDto :BaseFinancialRecordDto
	{
        public int? ClientID{ get; set; }
		public string? ClientName { get; set; }
	}
}
