namespace AFayedFarm.Global
{
	public class RequestResponse<T> : IDisposable
	{
		/// <summary>
		/// ResponseID = 1 == Done
		/// ResponseID = 2 == false with condition
		/// ResponseID = 3 == false with condition
		/// ResponseID = 0 == Error
		/// </summary>
		public int? ResponseID { get; set; }
		public string? ResponseMessage { get; set; }
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public int? LastPage { get; set; }
        public T? ResponseValue { get; set; }

        public void Dispose() { }
		
		
	}
}
