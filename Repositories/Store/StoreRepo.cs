using AFayedFarm.Dtos;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Repositories.Store
{
	public class StoreRepo : IStoreRepo
	{
		private readonly FarmContext context;

		public StoreRepo(FarmContext context)
        {
			this.context = context;
		}
        public async Task<RequestResponse<List<StoreProductDto>>> GetStoreProducts()
		{
			var response = new RequestResponse<List<StoreProductDto>> { ResponseID = 0, ResponseValue = new List<StoreProductDto>() };
			var productList = new List<StoreProductDto>();
			var productsInStore = await context.StoreProducts.Include(c=>c.Product).OrderBy(c=>c.ProductID).ToListAsync();
			if(productsInStore.Count != 0)
			{
				foreach (var item in productsInStore)
				{
					var product = new StoreProductDto();
					product.ProductID = item.ProductID;
					product.ProductName = item?.Product?.ProductName ?? "";
					product.Quantity = item.Quantity;
					product.Notes = item.Notes;

					productList.Add(product);
				}
				response.ResponseID = 1;
				response.ResponseValue = productList;
			}
			return response;
		}
	}
}
