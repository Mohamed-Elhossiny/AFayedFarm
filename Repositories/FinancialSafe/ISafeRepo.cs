using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.FinancialSafe
{
	public interface ISafeRepo
	{
		Task<RequestResponse<SafeDto>> GetSafeBalance();
		Task<RequestResponse<SafeDto>> AddBalance(decimal balance);
	}
}
