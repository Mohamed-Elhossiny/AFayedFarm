﻿using AFayedFarm.Dtos;
using AFayedFarm.Dtos.Financial;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.FinancialSafe
{
	public interface ISafeRepo
	{
		Task<RequestResponse<SafeDto>> GetSafeBalance();
		Task<RequestResponse<SafeDto>> AddBalance(BalanceDto dto);
		Task<RequestResponse<SafeDto>> Withdraw(WithdrawDto dto);
		Task<RequestResponse<List<FinancialEmployeeDto>>> GetEmployeeFinancialRecords(int pageNumber, int pageSize, DateTime? from, DateTime? to);
		Task<RequestResponse<List<FinancialClientDto>>> GetClientFinancialRecords(int pageNumber, int pageSize, DateTime? from, DateTime? to);
		Task<RequestResponse<List<FinancialFarmDto>>> GetFarmFinancialRecords(int pageNumber, int pageSize, DateTime? from, DateTime? to);
		Task<RequestResponse<List<FinancialExpenseDto>>> GetExpenseFinancialRecords(int pageNumber, int pageSize, DateTime? from, DateTime? to);
		Task<RequestResponse<List<FinancialFridgeDto>>> GetFridgeFinancialRecords(int pageNumber, int pageSize, DateTime? from, DateTime? to);
		Task<RequestResponse<List<AllFinancialRecordsDto>>> GetAllFinancialRecords(int pageNumber,int pageSize, DateTime? from, DateTime? to);
	}
}
