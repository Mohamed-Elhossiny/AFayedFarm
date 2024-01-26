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
		public async Task<IActionResult> GetStoreProducts(int currentPage = 1, int pageSize = 100)
		{
			var response =await storeRepo.GetStoreProducts(currentPage,pageSize);
			if (response.ResponseID == 1)
				//return Ok(response);
			return Ok(response.ResponseValue);
			else
				response.ResponseMessage = "There is no product in store";
				//return Ok(response);
				return NotFound();
		}

		[HttpPost("~/SetProductQtyToZero")]
		public async Task<IActionResult> SetProductQtyToZero(int id)
		{
			var response = await storeRepo.SetProductQtyToZero(id);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}

	}
}
