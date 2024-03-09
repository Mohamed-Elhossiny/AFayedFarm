using AFayedFarm.Dtos;
using AFayedFarm.Global;
using AFayedFarm.Repositories.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Authorize]
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
			var response = new RequestResponse<FarmDto> { ResponseID = 0, ResponseValue = new FarmDto() };
			if (farmDto == null)
				return BadRequest("Please Enter Farm Name");
			var farm = await farmsRepo.AddFarmAsync(farmDto);
			if (farm.ID != 0)
			{
				response.ResponseID = 1;
				response.ResponseValue = farm;
				return Ok(response);
			}
			response.ResponseMessage = "There is farm exists with the same name";
			return Ok(response);
		}

		[HttpGet("~/GetAllFarms")]
		public async Task<IActionResult> GetAllFarms(int pageNumber = 1, int pageSize = 500)
		{
			var response = await farmsRepo.GetFarmsAsync(pageNumber, pageSize);
			if (response.ResponseID == 1)
				return Ok(response);
			response.ResponseMessage = "No Data Found";
			return Ok(response);
		}

		[HttpGet("~/GetAllFarmsOffline")]
		public async Task<IActionResult> GetAllFarmsOffline(int pageNumber = 1, int pageSize = 500)
		{
			RequestResponse<List<FarmDto>>? response = await farmsRepo.GetFarmsAsync(pageNumber, pageSize);
			if (response.ResponseID == 1)
			{
				foreach (var item in response.ResponseValue!)
				{
					RequestResponse<FarmRecordsWithFarmDataDto> records = await farmsRepo.GetFarmRecordWithFarmDataByID((int)item.ID!, 1, 100);
					if (records.ResponseID == 1)
					{
						//response.ResponseValue.ForEach(f => f.OfflineRecords = records.ResponseValue!.FarmRecords);
						item.OfflineRecords = records.ResponseValue?.FarmRecords;
					}
				}
				return Ok(response);
			}
			response.ResponseMessage = "No Data Found";
			return Ok(response);
		}

		[HttpGet("~/GetFarmById")]
		public async Task<IActionResult> GetFarmById(int id)
		{
			var response = new RequestResponse<FarmDto> { ResponseID = 0, ResponseValue = new FarmDto() };
			var farmdb = await farmsRepo.GetFarmById(id);
			if (farmdb.ID == 0)
			{
				response.ResponseMessage = $"No Farm Found By ID {id}";
				return Ok(response);
			}
			return Ok(farmdb);
		}

		[HttpPut("~/UpdateFarm")]
		public async Task<IActionResult> UpdateFarm(int id, [FromBody] AddFarmDto farmDto)
		{
			if (farmDto.Name == "")
				return BadRequest("Please Enter Farm Name");
			var farmDb = await farmsRepo.GetFarmById(id);
			if (farmDb.ID == 0)
			{
				var responseRequest = new RequestResponse<FarmDto> { ResponseID = 0, ResponseValue = new FarmDto() };
				responseRequest.ResponseMessage = $"No farm found by this {id}";
				return Ok(responseRequest);
			}
			var response = await farmsRepo.UpdateFarm(id, farmDto);
			return Ok(response);
			//else
			//	return NotFound(response);
		}

		[HttpPost("~/AddFarmRecord")]
		public async Task<IActionResult> AddFarmRecordAsync([FromBody] AddFarmRecordDto farmdto)
		{
			if (farmdto.FarmsID == 0 || farmdto.ProductID == 0)
				return BadRequest("Please Enter Farm And Product");
			var response = await farmsRepo.AddFarmRecord(farmdto);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = "Error in adding record";
				return Ok(response);
			}
		}

		[HttpGet("~/GetAllFarmRecords")]
		public async Task<IActionResult> GetAllFarmsRecord(int farmid)
		{
			var response = await farmsRepo.GetAllFarmRecordsWithTotal(farmid);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = "No Data Found";
				return Ok(response);
			}
		}

		[HttpGet("~/GetFarmRecord")]
		public async Task<IActionResult> GetFarmsRecord(int recordId)
		{
			var response = await farmsRepo.GetFarmRecordByID(recordId);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = "No Data Found";
				return Ok(response);
			}
		}

		[HttpPut("~/UpdateFarmRecord")]
		public async Task<IActionResult> UpdateRecordFarm(int recordId, [FromBody] AddFarmRecordDto farmdto)
		{
			if (recordId == 0)
				return BadRequest("Please Select ID to update");
			var response = await farmsRepo.UpdateFarmRecordAsync(recordId, farmdto);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = "Error in update record";
				return Ok(response);
			}
		}


		[HttpGet("~/GetFarmRecordWithData")]
		public async Task<IActionResult> GetFarmsRecordWithData(int recordId, int pageNumber = 1, int pageSize = 500)
		{
			var response = await farmsRepo.GetFarmRecordWithFarmDataByID(recordId, pageNumber, pageSize);
			if (response.ResponseID == 1)
				return Ok(response);
			else if (response.ResponseValue?.ID == 0)
			{
				response.ResponseMessage = $"NO Farm With this {recordId}";
				return Ok(response);
			}
			else
				response.ResponseMessage = "There is no records";
			return Ok(response);
		}

		[HttpGet("~/AllProductsDetails")]
		public async Task<IActionResult> AllProductsDetails(int pageNumber = 1, int pageSize = 500)
		{
			var response = await farmsRepo.GetProductsDetails(pageNumber, pageSize);
			return Ok(response);
		}

		[HttpPost("~/PayToFarm")]
		public async Task<IActionResult> PayToFarm(FarmPaymentDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await farmsRepo.PayToFarm(dto);
			return Ok(response);
		}
	}
}
