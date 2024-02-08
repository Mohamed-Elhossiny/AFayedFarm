using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Products
{
	public interface IProductRepo
	{
		Task<RequestResponse<List<ProductDto>>> GetAllProducts(int currentPage, int pageSize);
		Task<RequestResponse<ProductDto>> AddProduct(AddNewProductDto dto);
		Task<RequestResponse<ProductDto>> UpdateProduct(int id, AddProductDto dto);
		Task<RequestResponse<bool>> AddProductToStore(AddProductDto dto,int id);

	}
}
