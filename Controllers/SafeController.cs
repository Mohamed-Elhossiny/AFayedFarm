using AFayedFarm.Dtos;
using AFayedFarm.Dtos.Financial;
using AFayedFarm.Global;
using AFayedFarm.Repositories.FinancialSafe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SafeController : ControllerBase
	{
		private readonly ISafeRepo safeRepo;

		public SafeController(ISafeRepo safeRepo)
		{
			this.safeRepo = safeRepo;
		}

		[HttpGet("~/GetTotalbalance")]
		public async Task<IActionResult> GetTotalbalance()
		{
			var response = await safeRepo.GetSafeBalance();
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}

		[HttpPost("~/AddBalanceToSafe")]
		public async Task<IActionResult> AddBalance(BalanceDto dto)
		{
			if (dto.Balance <= 0)
				return BadRequest("Enter valid balance");
			var response = await safeRepo.AddBalance(dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}

		[HttpPost("~/GetRecords")]
		public async Task<IActionResult> GetRecords(int pageNumber = 1, int pageSize = 500,int recordType = 0)
		{
			if (recordType < 0 || recordType > 5)
			{
				recordType = 0;
			}
			switch (recordType)
			{
				// All Records = 0
				case 0:
					var allRecords = await safeRepo.GetAllFinancialRecords(pageNumber, pageSize);
					if (allRecords.ResponseID == 1)
						return Ok(allRecords);
					else
						allRecords.ResponseMessage = "No Records";
						return Ok(allRecords);
						
				// Farms = 1
				case 1:
					var farmRecords = await safeRepo.GetFarmFinancialRecords(pageNumber, pageSize);
					if (farmRecords.ResponseID == 1)
						return Ok(farmRecords);
					else
						farmRecords.ResponseMessage = $"No Farm Records";
						return Ok(farmRecords);

				// Expense = 2
				case 2:
					var expenseRecords = await safeRepo.GetExpenseFinancialRecords(pageNumber, pageSize);
					if (expenseRecords.ResponseID == 1)
						return Ok(expenseRecords);
					else
						expenseRecords.ResponseMessage = "No Expense Records";
						return Ok(expenseRecords);

				// Fridge = 3
				case 3:
					var fridgeRecords = await safeRepo.GetFridgeFinancialRecords(pageNumber, pageSize);
					if (fridgeRecords.ResponseID == 1)
						return Ok(fridgeRecords);
					else
						fridgeRecords.ResponseMessage = "No Fridge Records";
						return Ok(fridgeRecords);

				// Client = 4
				case 4:
					var clientRecords = await safeRepo.GetClientFinancialRecords(pageNumber, pageSize);
					if (clientRecords.ResponseID == 1)
						return Ok(clientRecords);
					else
						clientRecords.ResponseMessage = "No Client Records";
						return Ok(clientRecords);

				// Employee = 5
				case 5:
					var employeeRecords = await safeRepo.GetEmployeeFinancialRecords(pageNumber, pageSize);
					if (employeeRecords.ResponseID == 1)
						return Ok(employeeRecords);
					else
						employeeRecords.ResponseMessage = "No Employee Records";
						return Ok(employeeRecords);
					
			}
			return Ok($"Please enter valid record type{recordType}");
			//return NotFound($"There is no financial transaction for that {recordType} && Page {pageNumber} && Page Size {pageSize}");

		}

	}
}
