using AFayedFarm.Repositories.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StoreController : ControllerBase
	{
		private readonly IStoreRepo storeRepo;

		public StoreController(IStoreRepo storeRepo)
        {
			this.storeRepo = storeRepo;
		}

		[HttpGet("~/GetStoreProducts")]
		public async Task<IActionResult> GetStoreProducts()
		{
			var response =await storeRepo.GetStoreProducts();
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}

	}
}
