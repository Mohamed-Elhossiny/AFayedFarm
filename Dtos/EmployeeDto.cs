namespace AFayedFarm.Dtos
{
	public class EmployeeDto
	{
        public int? ID { get; set; }
        public string? Name { get; set; }
		public decimal? Salary { get; set; }
        public decimal? Total { get; set; }
        public DateTime? Created_Date { get; set; }
    }
}
