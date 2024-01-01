using AFayedFarm.Dtos;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;
using System;

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
				var product = new Product()
				{
					ProductName = dto.Name,
					Created_Date = DateTime.Now.Date,
					ProductNote = dto.Notes
				};
				await context.Products.AddAsync(product);
				await context.SaveChangesAsync();
				var productDto = new ProductDto()
				{
					ID = product.ProductID,
					Name = product.ProductName,
					Notes = product.ProductNote,
					Created_Date = DateOnly.FromDateTime(product.Created_Date ?? DateTime.Now)
				};
				response.ResponseID = 1;
				response.ResponseValue = productDto;
			}
			return response;
		}

		public async Task<RequestResponse<List<ProductDto>>> GetAllProducts()
		{
			var response = new RequestResponse<List<ProductDto>> { ResponseID = 0, ResponseValue = new List<ProductDto>() };
			var productList = await context.Products.OrderByDescending(c => c.ProductID).ToListAsync();
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
						Created_Date = DateOnly.FromDateTime(item.Created_Date ?? DateTime.Now)
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
			if (productDb != null)
			{
				var createdDate = new DateTime();
				productDb.ProductName = dto.Name;
				productDb.ProductNote = dto.Notes;

				context.Products.Update(productDb);
				await context.SaveChangesAsync();

				var productDto = new ProductDto()
				{
					ID = productDb.ProductID,
					Name = productDb.ProductName,
					Notes = productDb.ProductNote,
					Created_Date = DateOnly.FromDateTime(productDb.Created_Date ?? DateTime.Now)
				};

				response.ResponseID = 1;
				response.ResponseValue = productDto;
			}
			return response;
		}
	}
}
