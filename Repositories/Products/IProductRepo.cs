using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Products
{
	public interface IProductRepo
	{
		Task<RequestResponse<List<ProductDto>>> GetAllProducts();
		Task<RequestResponse<ProductDto>> AddProduct(AddProductDto dto);
		Task<RequestResponse<ProductDto>> UpdateProduct(int id, AddProductDto dto);
	}
}
