namespace AFayedFarm.Dtos
{
	public class EmployeePaymentDto
	{
        public int? Id { get; set; }
        public decimal? Total { get; set; }
        public int? TrasactionTypeID { get; set; }
        public string? Notes { get; set; }
    }
}
