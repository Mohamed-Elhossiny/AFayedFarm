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
				return Ok(response);
			}
			response.ResponseMessage = "There is farm exists with the same name";
			return Ok(response);
		}

		[HttpGet("~/GetAllFridges")]
		public async Task<IActionResult> GetAllFridges()
		{
			var allFarms = await fridgeRepo.GetFridgesAsync();
			return Ok(allFarms);
		}

		[HttpGet("~/GetFridgeById")]
		public async Task<IActionResult> GetFridgeById(int id)
		{
			var response = await fridgeRepo.GetFridgeById(id);
			if (response.ResponseID == 0)
			{
				response.ResponseMessage = $"No Fridge Found By ID {id}";
				return Ok(response);
			}
			return Ok(response);
		}

		[HttpPut("~/UpdateFridge")]
		public async Task<IActionResult> UpdateFridge(int id, [FromBody] AddFridgeDto farmDto)
		{
			if (farmDto.Name == "")
				return BadRequest("Please Enter Farm Name");

			var fridgeDb = await fridgeRepo.GetFridgeById(id);
			if (fridgeDb.ResponseID == 0)
			{
				fridgeDb.ResponseMessage = $"No fridge found by this {id}";
				return Ok(fridgeDb);
			}

			var response = await fridgeRepo.UpdateFridge(id, farmDto);
			return Ok(response);

		}

		[HttpPost("~/AddFridgeRecord")]
		public async Task<IActionResult> AddFridgeRecordAsync([FromBody] AddFridgeRecordDto fridgedto)
		{
			if (fridgedto.FridgeID == 0 || fridgedto.ProductID == 0)
				return BadRequest("Please Enter Fridge And Product");
			var response = await fridgeRepo.AddFridgeRecord(fridgedto);
			return Ok(response);
		}

		[HttpGet("~/GetAllFridgeRecords")]
		public async Task<IActionResult> GetAllFridgeRecords(int id)
		{
			var response = await fridgeRepo.GetAllFridgeRecordsWithTotal(id);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = "No Data Found";
				return Ok(response);
			}
		}

		[HttpGet("~/GetFridgeRecord")]
		public async Task<IActionResult> GetFridgeRecord(int id)
		{
			var response = await fridgeRepo.GetFridgeRecordByID(id);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = "No Data Found";
				return Ok(response);
			}
		}

		[HttpPut("~/UpdateFridgeRecord")]
		public async Task<IActionResult> UpdateFridgeRecord(int recordId, [FromBody] AddFridgeRecordDto fridgedto)
		{
			if (recordId == 0)
				return BadRequest("Please Select ID to update");
			var response = await fridgeRepo.UpdateFridgeRecordAsync(recordId, fridgedto);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = "Error in Update Fridge";
				return Ok(response);
			}
		}

		[HttpGet("~/GetFridgeRecordsWithData")]
		public async Task<IActionResult> GetFridgeRecordWithData(int fridgeId, int pageNumber = 1, int pageSize = 500)
		{
			var response = await fridgeRepo.GetFridgeRecordWithFridgeDataByID(fridgeId, pageNumber, pageSize);
			if (response.ResponseID == 1)
				return Ok(response);

			else
				response.ResponseMessage = "There is on records";
			return Ok(response);
		}

		[HttpPost("~/PayToFridge")]
		public async Task<IActionResult> PayToFridge(FridgePaymentDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await fridgeRepo.PayToFridge(dto);
			return Ok(response);
		}
	}
}
