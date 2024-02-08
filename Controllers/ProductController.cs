using AFayedFarm.Dtos;
using AFayedFarm.Global;
using AFayedFarm.Repositories.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductRepo productRepo;

		public ProductController(IProductRepo productRepo)
		{
			this.productRepo = productRepo;
		}

		[HttpGet("~/GetAllProducts")]
		public async Task<IActionResult> GetAllProducts(int currentPage = 1, int pageSize = 100)
		{
			var response = await productRepo.GetAllProducts(currentPage,pageSize);
			if (response.ResponseID == 1)
				//return Ok(response);
				return Ok(response.ResponseValue);
			else
				response.ResponseMessage = "There is no products";
				//return Ok(response);
				return NotFound();
		}

		[HttpPost("~/AddNewProduct")]
		public async Task<IActionResult> AddNewProduct(AddNewProductDto dto)
		{
			var response = await productRepo.AddProduct(dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else 
				return NotFound();
		}

		[HttpPut("~/UpdateProduct")]
		public async Task<IActionResult> UpdateProduct(int id,AddProductDto dto)
		{
			if (id == 0)
				return BadRequest();
			var response = await productRepo.UpdateProduct(id, dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}
	}
}
