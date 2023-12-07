using AFayedFarm.Dtos;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Repositories.Products
{
	public class ProductRepo : IProductRepo
	{
		private readonly FarmContext context;

		public ProductRepo(FarmContext context)
		{
			this.context = context;
		}
		public async Task<RequestResponse<ProductDto>> AddProduct(AddProductDto dto)
		{
			var response = new RequestResponse<ProductDto> { ResponseID = 0 };
			var productDB = await context.Products.Where(c => c.ProductName.ToLower() == dto.Name.ToLower()).SingleOrDefaultAsync();
			if (productDB == null)
			{
				var createdDate = new DateTime();
				var product = new Product()
				{
					ProductName = dto.Name,
					Created_Date = DateTime.TryParse(dto.Date, out createdDate) == true ? createdDate : createdDate,
					ProductNote = dto.Notes
				};
				await context.Products.AddAsync(product);
				await context.SaveChangesAsync();
				var productDto = new ProductDto()
				{
					ID = product.ProductID,
					Name = product.ProductName,
					Notes = product.ProductNote,
					Date = product.Created_Date.Value.ToShortDateString()
				};
				response.ResponseID = 1;
				response.ResponseValue = productDto;
			}
			return response;
		}

		public async Task<RequestResponse<List<ProductDto>>> GetAllProducts()
		{
			var response = new RequestResponse<List<ProductDto>> { ResponseID = 0, ResponseValue = new List<ProductDto>() };
			var productList = await context.Products.ToListAsync();
			if (productList.Count != 0)
			{
				var productListDto = new List<ProductDto>();
				foreach (var item in productList)
				{
					var productDto = new ProductDto
					{
						ID = item.ProductID,
						Name = item.ProductName,
						Notes = item.ProductNote,
						Date = item.Created_Date.Value.ToShortDateString()
					};
					productListDto.Add(productDto);
				}
				response.ResponseID = 1;
				response.ResponseValue = productListDto;
			}
			return response;
		}

		public async Task<RequestResponse<ProductDto>> UpdateProduct(int id, AddProductDto dto)
		{
			var response = new RequestResponse<ProductDto> { ResponseID = 0 };
			var productDb = await context.Products.Where(c => c.ProductID == id).FirstOrDefaultAsync();
			if(productDb != null)
			{
				var createdDate = new DateTime();
				productDb.ProductName = dto.Name;
				productDb.Created_Date = DateTime.TryParse(dto.Date, out createdDate) == true ? createdDate : createdDate;
				productDb.ProductNote = dto.Notes;

				context.Products.Update(productDb);
				await context.SaveChangesAsync();

				var productDto = new ProductDto()
				{
					ID = productDb.ProductID,
					Name = productDb.ProductName,
					Notes = productDb.ProductNote,
					Date = productDb.Created_Date.Value.ToShortDateString()
				};

				response.ResponseID = 1;
				response.ResponseValue = productDto;
			}
			return response;
		}
	}
}
