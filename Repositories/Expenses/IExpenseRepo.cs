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
	}
}
