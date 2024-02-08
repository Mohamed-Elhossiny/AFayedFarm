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
		public async Task<RequestResponse<ProductDto>> AddProduct(AddNewProductDto dto)
		{
			var response = new RequestResponse<ProductDto> { ResponseID = 0 };
			var productDB = await context.Products.Where(c => c.ProductName.ToLower() == dto.Name.ToLower()).SingleOrDefaultAsync();
			if (productDB == null)
			{
				var product = new Product()
				{
					ProductName = dto.Name,
					Created_Date = DateTime.Now.Date,
					//ProductNote = dto.Notes
				};
				await context.Products.AddAsync(product);
				await context.SaveChangesAsync();

				//await AddProductToStore(dto, product.ProductID);

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

		public async Task<RequestResponse<List<ProductDto>>> GetAllProducts(int currentPage = 1,int pageSize = 100)
		{
			var response = new RequestResponse<List<ProductDto>> { ResponseID = 0, ResponseValue = new List<ProductDto>() };
			var productLists = await context.Products.OrderByDescending(c => c.Created_Date).ToListAsync();

			var productList = productLists.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

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

				response.LastPage = (int)Math.Ceiling((double)productLists.Count() / pageSize);
				response.CurrentPage = currentPage;
				response.PageSize = pageSize;

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
				//productDb.ProductNote = dto.Notes;

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

		public async Task<RequestResponse<bool>> AddProductToStore(AddProductDto dto,int id)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			var productInStore = await context.StoreProducts.Where(c => c.ProductID == id).FirstOrDefaultAsync();
			if (productInStore != null)
			{
				if (productInStore.Quantity == null)
					productInStore.Quantity = 0;
				productInStore.Quantity += dto.Quantity;
				context.StoreProducts.Update(productInStore);
				await context.SaveChangesAsync();
			}
			else
			{
				var storeProduct = new StoreProduct();
				if (dto != null)
				{
					storeProduct.ProductID = id;
					storeProduct.StoreID = 2;
					storeProduct.Created_Date = DateTime.Now.Date;
					storeProduct.Quantity = dto.Quantity;

					await context.StoreProducts.AddAsync(storeProduct);
					await context.SaveChangesAsync();
				}
			}
			response.ResponseID = 1;
			response.ResponseValue = true;
			return response;
		}
	}
}
