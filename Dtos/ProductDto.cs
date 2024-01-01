namespace AFayedFarm.Dtos
{
	public class ProductDto
	{
        public int ID { get; set; }
        public string? Name { get; set; }
        public DateOnly? Created_Date { get; set; }
        public string? Notes { get; set; }
    }
}
