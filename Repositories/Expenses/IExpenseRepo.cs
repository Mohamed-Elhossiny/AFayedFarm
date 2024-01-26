using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Expenses
{
	public interface IExpenseRepo
	{
		Task<RequestResponse<ExpenseDto>> AddExpenseAsync(AddExpenseDto expenseDto);
		Task<RequestResponse<List<ExpenseDto>>> GetExpenseAsync();
		Task<RequestResponse<ExpenseDto>> GetExpenseByID(int id);
		Task<RequestResponse<ExpenseDto>> UpdateExpenseAsync(int id, AddExpenseDto expenseDto);
		Task<RequestResponse<ExpenseTypeDto>> AddExpenseTypeAsync(AddExpenseTypeDto dto);
		Task<RequestResponse<List<ExpenseTypeDto>>> GetAllExpenseTypes();
		Task<RequestResponse<ExpenseRecordDto>> AddExpenseRecordAsync(AddExpenseRecordDto dto);
		Task<RequestResponse<ExpenseRecordDto>> GetExpenseRecordById(int id);
		Task<RequestResponse<ExpenseRecordDto>> UpdateExpenseRecord(int id, AddExpenseRecordDto dto);

		Task<RequestResponse<ExpenseRecordsWithDataDto>> GetExpensesRecordsWithDataByExpenseId(int expenseId,int pageNumber = 1, int pageSize = 100);
		Task<RequestResponse<List<ExpenseRecordDto>>> GetExpensesForFarmRecord(int farmRecordID);
		Task<RequestResponse<List<ExpenseRecordDto>>> GetAllExpenseRecordsByExpenseId(int expenseID);
		Task<RequestResponse<ExpenseDto>> PayToExpense(ExpensePaymentDto dto);
	}
}
