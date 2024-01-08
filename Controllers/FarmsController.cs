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

		[HttpPost("~/AddFarm")]
		public async Task<IActionResult> AddFarmAsync(AddFarmDto farmDto)
		{
			if (farmDto == null)
				return BadRequest("Please Enter Farm Name");
			var farm = await farmsRepo.AddFarmAsync(farmDto);
			if (farm.ID != 0)
			{
				return Ok(farm);
			}
			return Conflict("There is farm exists with the same name");
		}

		[HttpGet("~/GetAllFarms")]
		public async Task<IActionResult> GetAllFarms()
		{
			var allFarms = await farmsRepo.GetFarmsAsync();
			if (allFarms.Count() != 0)
				return Ok(allFarms);
			return Conflict("No Data Found");
		}

		[HttpGet("~/GetFarmById")]
		public async Task<IActionResult> GetFarmById(int id)
		{
			var farmdb = await farmsRepo.GetFarmById(id);
			if (farmdb.ID == 0)
				return NotFound($"No Farm Found By ID {id}");
			return Ok(farmdb);
		}

		[HttpPut("~/UpdateFarm")]
		public async Task<IActionResult> UpdateFarm(int id, [FromBody] AddFarmDto farmDto)
		{
			var farmDb = await farmsRepo.GetFarmById(id);
			if (farmDb.ID == 0)
				return NotFound($"No farm found by this {id}");
			if (farmDto.Name == "")
				return BadRequest("Please Enter Farm Name");

			var response = await farmsRepo.UpdateFarm(id, farmDto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound(response.ResponseValue);
		}

		[HttpPost("~/AddFarmRecord")]
		public async Task<IActionResult> AddFarmRecordAsync([FromBody] AddFarmRecordDto farmdto)
		{
			if (farmdto.FarmsID == 0 || farmdto.ProductID == 0)
				return BadRequest("Please Enter Farm And Product");
			var response = await farmsRepo.AddFarmRecord(farmdto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return BadRequest();
		}

		[HttpGet("~/GetAllFarmRecords")]
		public async Task<IActionResult> GetAllFarmsRecord(int farmid)
		{
			var response = await farmsRepo.GetAllFarmRecordsWithTotal(farmid);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound("No Data Found");
		}

		[HttpGet("~/GetFarmRecord")]
		public async Task<IActionResult> GetFarmsRecord(int recordId)
		{
			var response = await farmsRepo.GetFarmRecordByID(recordId);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound("No Data Found");
		}

		[HttpPut("~/UpdateFarmRecord")]
		public async Task<IActionResult> UpdateRecordFarm(int recordId, [FromBody] AddFarmRecordDto farmdto)
		{
			if (recordId == 0)
				return BadRequest("Please Select ID to update");
			var response = await farmsRepo.UpdateFarmRecordAsync(recordId, farmdto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return BadRequest(farmdto);
		}

		//[HttpPost("~/GetTotalRemaining")]
		//public async Task<IActionResult> GetTotalRemaining(int farmID)
		//{
		//	var response = await farmsRepo.CalculateTotalRemainingFromRecords(farmID);
		//	if (response.ResponseID == 1)
		//		return Ok(response.ResponseValue);
		//	else
		//		return NotFound(response.ResponseValue);
		//}

		[HttpGet("~/GetFarmRecordWithData")]
		public async Task<IActionResult> GetFarmsRecordWithData(int recordId)
		{
			var response = await farmsRepo.GetFarmRecordWithFarmDataByID(recordId);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else if (response.ResponseValue.ID == 0)
				return NotFound($"NO Farm With this {recordId}");
			else
				return Ok(response.ResponseValue);
		}

		[HttpGet("~/AllProductsDetails")]
		public async Task<IActionResult> AllProductsDetails(/*int id*/)
		{
			//if (id == 0)
			//	return BadRequest("Enter Valid ID");
			var response = await farmsRepo.GetProductsDetails();
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound(response.ResponseValue);

		}

		[HttpPost("~/PayToFarm")]
		public async Task<IActionResult> PayToFarm(FarmPaymentDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await farmsRepo.PayToFarm(dto);
			if (response.ResponseID == 0)
				return NotFound();
			else
				return Ok(response.ResponseValue);
		}
	}
}
