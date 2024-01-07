using AFayedFarm.Dtos;
using AFayedFarm.Dtos.Financial;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.FinancialSafe
{
	public interface ISafeRepo
	{
		Task<RequestResponse<SafeDto>> GetSafeBalance();
		Task<RequestResponse<SafeDto>> AddBalance(BalanceDto dto);
		Task<RequestResponse<List<FinancialEmployeeDto>>> GetEmployeeFinancialRecords();
		Task<RequestResponse<List<FinancialClientDto>>> GetClientFinancialRecords();
		Task<RequestResponse<List<FinancialFarmDto>>> GetFarmFinancialRecords();
		Task<RequestResponse<List<FinancialExpenseDto>>> GetExpenseFinancialRecords();
		Task<RequestResponse<List<FinancialFridgeDto>>> GetFridgeFinancialRecords();
		Task<RequestResponse<List<AllFinancialRecordsDto>>> GetAllFinancialRecords();
	}
}
