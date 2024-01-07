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
		public async Task<IActionResult> GetRecords(int recordType = 0)
		{
			if (recordType < 0 || recordType > 5)
			{
				recordType = 0;
			}
			switch (recordType)
			{
				// All Records = 0
				case 0:
					var allRecords = await safeRepo.GetAllFinancialRecords();
					if (allRecords.ResponseID == 1)
						return Ok(allRecords.ResponseValue);
					else
						break;
				// Farms = 1
				case 1:
					var farmRecords = await safeRepo.GetFarmFinancialRecords();
					if (farmRecords.ResponseID == 1)
						return Ok(farmRecords.ResponseValue);
					else
						break;

				// Expense = 2
				case 2:
					var expenseRecords = await safeRepo.GetExpenseFinancialRecords();
					if (expenseRecords.ResponseID == 1)
						return Ok(expenseRecords.ResponseValue);
					else
						break;

				// Fridge = 3
				case 3:
					var fridgeRecords = await safeRepo.GetFridgeFinancialRecords();
					if (fridgeRecords.ResponseID == 1)
						return Ok(fridgeRecords.ResponseValue);
					else
						break;

				// Client = 4
				case 4:
					var clientRecords = await safeRepo.GetClientFinancialRecords();
					if (clientRecords.ResponseID == 1)
						return Ok(clientRecords.ResponseValue);
					else
						break;

				// Employee = 5
				case 5:
					var employeeRecords = await safeRepo.GetEmployeeFinancialRecords();
					if (employeeRecords.ResponseID == 1)
						return Ok(employeeRecords.ResponseValue);
					else
						break;
			}
			return NotFound($"There is no financial transaction for that {recordType}");

		}

	}
}
