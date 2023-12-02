using AFayedFarm.Dtos;
using AFayedFarm.Repositories.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FarmsController : ControllerBase
	{
		private readonly IFarmsRepo farmsRepo;

		public FarmsController(IFarmsRepo farmsRepo)
		{
			this.farmsRepo = farmsRepo;
		}
		
		[HttpPost]
		public async Task<IActionResult> AddFarmAsync(AddFarmDto farmDto)
		{
			if (farmDto == null)
				return BadRequest("Please Enter Farm Name");
			var farm = await farmsRepo.AddFarmAsync(farmDto);
			if (farm.FarmID != 0)
			{
				return Ok(farm);
			}
			return Conflict("There is farm exists with the same name");
		}

		[HttpGet]
		public async Task<IActionResult> GetAllFarms()
		{
			var allFarms = await farmsRepo.GetFarmsAsync();
			if (allFarms.Count() != 0)
				return Ok(allFarms);
			return Conflict("No Data Found");
		}

		[HttpGet("id:int")]
		public async Task<IActionResult> GetFarmById(int id)
		{
			var farmdb = await farmsRepo.GetFarmById(id);
			if (farmdb.FarmID == 0)
				return NotFound($"No Farm Found By ID {id}");
			return Ok(farmdb);
		}

		[HttpPut("id:int")]
		public async Task<IActionResult> UpdateFarm(int id, [FromBody]AddFarmDto farmDto)
		{
			var farmDb = await farmsRepo.GetFarmById(id);
			if (farmDb.FarmID == 0)
				return NotFound($"No farm found by this {id}");
			if (farmDto.FarmName == "")
				return BadRequest("Please Enter Farm Name");

			var farmUpdated = await farmsRepo.UpdateFarm(id,farmDto);
			return Ok(farmUpdated);
		}

		[HttpPost("AddFarmRecord")]
		public async Task<IActionResult> AddFarmRecordAsync([FromBody] AddFarmRecordDto farmdto)
		{
			if (farmdto.FarmsID == 0 || farmdto.ProductID == 0)
				return BadRequest("Please Enter Farm And Product");
			var response = await farmsRepo.AddFarmRecord(farmdto);
			if (response.ResponseID == 1)
				return Ok(farmdto);
			else
				return BadRequest();
		}

		[HttpGet("GetAllFarmsRecord")]
		public async Task<IActionResult> GetAllFarmsRecord(int id)
		{
			var response = await farmsRepo.GetAllFarmRecords(id);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound("No Data Found");
		}
	}
}
