using AFayedFarm.Dtos;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Repositories.FinancialSafe
{
	public class SafeRepo : ISafeRepo
	{
		private readonly FarmContext context;

		public SafeRepo(FarmContext context)
		{
			this.context = context;
		}

		public async Task<RequestResponse<SafeDto>> AddBalance(decimal balance)
		{
			var response = new RequestResponse<SafeDto> { ResponseID = 0, ResponseValue = new SafeDto() };
			var safeDb = await context.Safe.FirstOrDefaultAsync();
			if (safeDb != null)
			{
				safeDb.Total += balance;
				await context.SaveChangesAsync();

				var safeDto = await GetSafeBalance();

				response.ResponseID = 1;
				response.ResponseValue = safeDto.ResponseValue;
			}
			return response;
		}

		public async Task<RequestResponse<SafeDto>> GetSafeBalance()
		{
			var response = new RequestResponse<SafeDto> { ResponseID = 0, ResponseValue = new SafeDto() };
			var safeDb = await context.Safe.FirstOrDefaultAsync();
			if (safeDb != null)
			{
				var safe = new SafeDto
				{
					ID = safeDb.ID,
					Total = safeDb.Total,
				};

				response.ResponseValue = safe;
				response.ResponseID = 1;
			}
			return response;
		}
	}
}
