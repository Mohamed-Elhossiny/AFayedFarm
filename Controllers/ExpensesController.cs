using AFayedFarm.Dtos;
using AFayedFarm.Repositories.Expenses;
using AFayedFarm.Repositories.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExpensesController : ControllerBase
	{
		private readonly IExpenseRepo expenseRepo;

		public ExpensesController(IExpenseRepo expenseRepo)
        {
			this.expenseRepo = expenseRepo;
		}
		[HttpPost("~/AddExpense")]
		public async Task<IActionResult> AddExpenseAsync(AddExpenseDto expenseDto)
		{
			if (expenseDto.Name == null || expenseDto.Name == "")
				return BadRequest("Please Enter Expene Name");
			if (expenseDto?.ExpenseTypeId == null || expenseDto.ExpenseTypeId == 0)
				return BadRequest("Please Enter Expene Type");

			var response = await expenseRepo.AddExpenseAsync(expenseDto);
			if (response.ResponseID != 0)
			{
				return Ok(response.ResponseValue);
			}
			return BadRequest("There is Exepnse exists with the same name");
		}

		[HttpGet("~/GetAllExpenses")]
		public async Task<IActionResult> GetAllExpenses()
		{
			var response = await expenseRepo.GetExpenseAsync();
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			return Ok(response.ResponseValue);
		}

		[HttpGet("~/GetExpenseById")]
		public async Task<IActionResult> GetExpenseById(int id)
		{
			var response = await expenseRepo.GetExpenseByID(id);
			if (response.ResponseID == 0)
				return NotFound($"No Exepnse Found By ID {id}");
			return Ok(response.ResponseValue);
		}

		[HttpPut("~/UpdateExpense")]
		public async Task<IActionResult> UpdateExpense(int id, [FromBody] AddExpenseDto expenseDto)
		{
			if (expenseDto.Name == "")
				return BadRequest("Please enter expense name");
			if (id == 0)
				return BadRequest("Please enter valid ID");
			var response = await expenseRepo.GetExpenseByID(id);
			if (response.ResponseID == 0)
				return NotFound($"No expense found by this {id}");

			var requestResponse = await expenseRepo.UpdateExpenseAsync(id, expenseDto);
			if (requestResponse.ResponseID == 1)
				return Ok(requestResponse.ResponseValue);
			return Conflict("Can't Updated Expense");
		}

		[HttpPost("~/AddExpenseType")]
		public async Task<IActionResult> AddExpenseTypeAsync(AddExpenseTypeDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (dto.ExpenseTypeName == null || dto.ExpenseTypeName == "")
				return BadRequest("Please enter type name");
			var response = await expenseRepo.AddExpenseTypeAsync(dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}

		[HttpGet("~/GetAllExpensetypes")]
		public async Task<IActionResult> GetAllExepenseTypes()
		{
			var response = await expenseRepo.GetAllExpenseTypes();
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return Ok(response.ResponseValue);
		}
		
		[HttpPost("~/AddExpenseRecord")]
		public async Task<IActionResult> AddExpenseRecord(AddExpenseRecordDto dto)
		{
			if (dto.ExpenseID == 0 || dto.ExpenseID == null)
				return BadRequest();
			if (dto == null)
				return BadRequest();
			var response = await expenseRepo.AddExpenseRecordAsync(dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound(dto);
		}

		[HttpGet("~/GetExpenseRecordById")]
		public async Task<IActionResult> GetExpenseRecordById(int id)
		{
			var response = await expenseRepo.GetExpenseRecordById(id);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound($"No Expense Record with ID {id}");
		}

		[HttpPut("~/UpdateExpenseRecord")]
		public async Task<IActionResult> UpdateExpenseRecord(int id, AddExpenseRecordDto dto)
		{
			if (id == 0 || id == null)
				return BadRequest();
			if (dto.ExpenseID == null || dto.ExpenseID == 0)
				return BadRequest();
			var response = await expenseRepo.UpdateExpenseRecord(id,dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound(dto); 
		}

		[HttpGet("~/GetExpensesForFarmRecord")]
		public async Task<IActionResult> GetExpensesForFarmRecord(int id)
		{
			if (id == 0)
				return BadRequest("Please enter valid record id");
			var response = await expenseRepo.GetExpensesForFarmRecord(id);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound(response.ResponseValue);
		}

		[HttpGet("~/GetExpensesWithData")]
		public async Task<IActionResult> GetExpensesWithData(int id,int pageNumber = 1,int pageSize = 100)
		{
			if (id == 0)
				return BadRequest("There is no data for this id");
			var response = await expenseRepo.GetExpensesRecordsWithDataByExpenseId(id,pageNumber,pageSize);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				response.ResponseMessage = "There is no records";
				return Ok(response.ResponseValue);
		}

		[HttpPost("~/PayToExpense")]
		public async Task<IActionResult> PayToExpense(ExpensePaymentDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await expenseRepo.PayToExpense(dto);
			if (response.ResponseID == 0)
				return NotFound();
			else
				return Ok(response.ResponseValue);
		}

	}
}
