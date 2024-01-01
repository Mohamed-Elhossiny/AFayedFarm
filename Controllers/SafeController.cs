using AFayedFarm.Dtos;
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

	}
}
