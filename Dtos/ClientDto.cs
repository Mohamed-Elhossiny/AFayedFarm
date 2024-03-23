using AFayedFarm.Dtos.Client;

namespace AFayedFarm.Dtos
{
	public class ClientDto
	{
		public int? ID { get; set; }
		public string? Name { get; set; }
		public decimal? Total { get; set; }
		public DateTime? Created_Date { get; set; }
		public List<TransactionMainDataDto>? OfflineRecords { get; set; } = new List<TransactionMainDataDto>();
	}
}
