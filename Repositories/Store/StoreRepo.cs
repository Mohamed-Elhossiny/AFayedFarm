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
		public async Task<RequestResponse<List<StoreProductDto>>> GetStoreProducts(int currentPage,int pageSize)
		{
			var response = new RequestResponse<List<StoreProductDto>> { ResponseID = 0, ResponseValue = new List<StoreProductDto>() };
			var productList = new List<StoreProductDto>();
			var productsInStores = await context.StoreProducts.Include(c => c.Product).OrderByDescending(c => c.ProductID).ToListAsync();

			var productsInStore = productsInStores.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (productsInStore.Count != 0)
			{
				foreach (var item in productsInStore)
				{
					var product = new StoreProductDto();
					product.ProductID = item.ProductID;
					product.ProductName = item?.Product?.ProductName ?? "";
					product.Quantity = item?.Quantity ?? 0;
					product.Notes = item?.Notes ?? "";

					productList.Add(product);
				}
				response.LastPage = (int)Math.Ceiling((double)productsInStores.Count() / pageSize);
				response.CurrentPage = currentPage;
				response.PageSize = pageSize;
				response.ResponseID = 1;
				response.ResponseValue = productList;
			}
			return response;
		}

		public async Task<RequestResponse<StoreProductDto>> SetProductQtyToZero(int productID)
		{
			var response = new RequestResponse<StoreProductDto> { ResponseID = 0, ResponseValue = new StoreProductDto() };
			var productsInStore = await context.StoreProducts.Include(c => c.Product).Where(s => s.ProductID == productID).FirstOrDefaultAsync();
			if (productsInStore != null)
			{
				productsInStore.Quantity = 0;
				context.StoreProducts.Update(productsInStore);
				await context.SaveChangesAsync();

				response.ResponseID = 1;
				response.ResponseValue.ProductName = productsInStore.Product!.ProductName;
				response.ResponseValue.ProductID = productsInStore.Product!.ProductID;
				response.ResponseValue.Notes = productsInStore.Notes;
				response.ResponseValue.Quantity = productsInStore.Quantity;
			}
			return response;
		}
	}
}
