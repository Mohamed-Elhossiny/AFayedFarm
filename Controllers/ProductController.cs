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
		public async Task<IActionResult> GetAllProducts()
		{
			var response = await productRepo.GetAllProducts();
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}

		[HttpPost("~/AddNewProduct")]
		public async Task<IActionResult> AddNewProduct(AddProductDto dto)
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
