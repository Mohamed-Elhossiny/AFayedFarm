using AFayedFarm.Dtos;
using AFayedFarm.Repositories.Fridges;
using AFayedFarm.Repositories.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FridgeController : ControllerBase
	{
		private readonly IFridgeRepo fridgeRepo;

		public FridgeController(IFridgeRepo fridgeRepo)
		{
			this.fridgeRepo = fridgeRepo;
		}

		[HttpPost("~/AddFridge")]
		public async Task<IActionResult> AddFridgeAsync(AddFridgeDto fridgeDto)
		{
			if (fridgeDto == null || fridgeDto.Name == "")
				return BadRequest("Please Enter Fridge Name");
			var response = await fridgeRepo.AddFridgeAsync(fridgeDto);
			if (response.ResponseID != 0)
			{
				return Ok(response.ResponseValue);
			}
			return Conflict("There is farm exists with the same name");
		}

		[HttpGet("~/GetAllFridges")]
		public async Task<IActionResult> GetAllFridges()
		{
			var allFarms = await fridgeRepo.GetFridgesAsync();
			if (allFarms.ResponseID != 0)
				return Ok(allFarms.ResponseValue);
			return Ok(allFarms.ResponseValue);
		}

		[HttpGet("~/GetFridgeById")]
		public async Task<IActionResult> GetFridgeById(int id)
		{
			var response = await fridgeRepo.GetFridgeById(id);
			if (response.ResponseID == 0)
				return NotFound($"No Fridge Found By ID {id}");
			return Ok(response.ResponseValue);
		}

		[HttpPut("~/UpdateFridge")]
		public async Task<IActionResult> UpdateFridge(int id, [FromBody] AddFridgeDto farmDto)
		{
			var fridgeDb = await fridgeRepo.GetFridgeById(id);
			if (fridgeDb.ResponseID == 0)
				return NotFound($"No fridge found by this {id}");
			if (farmDto.Name == "")
				return BadRequest("Please Enter Farm Name");

			var response = await fridgeRepo.UpdateFridge(id, farmDto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound(response.ResponseValue);
		}

		[HttpPost("~/AddFridgeRecord")]
		public async Task<IActionResult> AddFridgeRecordAsync([FromBody] AddFridgeRecordDto fridgedto)
		{
			if (fridgedto.FridgeID == 0 || fridgedto.ProductID == 0)
				return BadRequest("Please Enter Fridge And Product");
			var response = await fridgeRepo.AddFridgeRecord(fridgedto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return BadRequest();
		}

		[HttpGet("~/GetAllFridgeRecords")]
		public async Task<IActionResult> GetAllFridgeRecords(int id)
		{
			var response = await fridgeRepo.GetAllFridgeRecordsWithTotal(id);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound("No Data Found");
		}

		[HttpGet("~/GetFridgeRecord")]
		public async Task<IActionResult> GetFridgeRecord(int id)
		{
			var response = await fridgeRepo.GetFridgeRecordByID(id);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound("No Data Found");
		}

		[HttpPut("~/UpdateFridgeRecord")]
		public async Task<IActionResult> UpdateFridgeRecord(int recordId, [FromBody] AddFridgeRecordDto fridgedto)
		{
			if (recordId == 0)
				return BadRequest("Please Select ID to update");
			var response = await fridgeRepo.UpdateFridgeRecordAsync(recordId, fridgedto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return BadRequest(fridgedto);
		}

		[HttpGet("~/GetFridgeRecordsWithData")]
		public async Task<IActionResult> GetFridgeRecordWithData(int fridgeId,int pageNumber = 1,int pageSize = 100)
		{
			var response = await fridgeRepo.GetFridgeRecordWithFridgeDataByID(fridgeId,pageNumber,pageSize);
			if (response.ResponseID == 1)
				//return Ok(response);
			return Ok(response.ResponseValue);
			else
				response.ResponseMessage = "There is on records";
				return Ok(response.ResponseValue);
		}

		[HttpPost("~/PayToFridge")]
		public async Task<IActionResult> PayToFridge(FridgePaymentDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await fridgeRepo.PayToFridge(dto);
			if (response.ResponseID == 0)
				return NotFound();
			else
				return Ok(response.ResponseValue);
		}
	}
}
