﻿using AFayedFarm.Dtos;
using AFayedFarm.Global;
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
				return Ok(response);
			}
			response.ResponseMessage = $"There is Exepnse exists with the same name";
			return Ok(response);
		}

		[HttpGet("~/GetAllExpenses")]
		public async Task<IActionResult> GetAllExpenses(int pageNumber = 1, int pageSize = 500)
		{
			var response = await expenseRepo.GetExpenseAsync(pageNumber,pageSize);
			return Ok(response);
		}

		[HttpGet("~/GetAllExpensesOffline")]
		public async Task<IActionResult> GetAllExpensesOffline(int pageNumber = 1, int pageSize = 500)
		{
			var response = await expenseRepo.GetExpenseAsync(pageNumber, pageSize);
			if (response.ResponseID == 1)
			{
				foreach (var item in response.ResponseValue)
				{
					var records = await expenseRepo.GetExpensesRecordsWithDataByExpenseId(item.ID, pageNumber, pageSize);
					if (records.ResponseID == 1)
					{
						item.OfflineRecords = records.ResponseValue?.ExpensesList;
					}
				}
				return Ok(response);
			}
			response.ResponseMessage = "No Data Found";
			return Ok(response);
		}

		[HttpGet("~/GetExpenseById")]
		public async Task<IActionResult> GetExpenseById(int id)
		{
			var response = await expenseRepo.GetExpenseByID(id);
			if (response.ResponseID == 0)
			{
				response.ResponseMessage = $"No Exepnse Found By ID {id}";
				return Ok(response);
			}
			return Ok(response);
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
			{
				response.ResponseMessage = $"No expense found by this {id}";
				return Ok(response);
			}
			var requestResponse = await expenseRepo.UpdateExpenseAsync(id, expenseDto);
			return Ok(requestResponse);
		}

		[HttpPost("~/AddExpenseType")]
		public async Task<IActionResult> AddExpenseTypeAsync(AddExpenseTypeDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (dto.ExpenseTypeName == null || dto.ExpenseTypeName == "")
				return BadRequest("أدخل فئة المصروف");
			var response = await expenseRepo.AddExpenseTypeAsync(dto);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = "يوجد مصروف بهذا الاسم";
				return Ok(response);
			}
		}

		[HttpGet("~/GetAllExpensetypes")]
		public async Task<IActionResult> GetAllExepenseTypes()
		{
			var response = await expenseRepo.GetAllExpenseTypes();
			return Ok(response);
		}

		[HttpPost("~/AddExpenseRecord")]
		public async Task<IActionResult> AddExpenseRecord(AddExpenseRecordDto dto)
		{
			if (dto.ExpenseID == 0 || dto.ExpenseID == null)
				return BadRequest();
			if (dto == null)
				return BadRequest();
			var response = await expenseRepo.AddExpenseRecordAsync(dto);
			return Ok(response);

		}

		[HttpGet("~/GetExpenseRecordById")]
		public async Task<IActionResult> GetExpenseRecordById(int id)
		{
			var response = await expenseRepo.GetExpenseRecordById(id);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = $"No Expense Record with ID {id}";
				return Ok(response);
			}
		}

		[HttpPut("~/UpdateExpenseRecord")]
		public async Task<IActionResult> UpdateExpenseRecord(int id, AddExpenseRecordDto dto)
		{
			if (id == 0)
				return BadRequest();
			if (dto.ExpenseID == null || dto.ExpenseID == 0)
				return BadRequest();
			var response = await expenseRepo.UpdateExpenseRecord(id, dto);
			return Ok(response);

		}

		[HttpPut("~/UpdateExpenseType")]
		public async Task<IActionResult> UpdateExpenseTypeAsync(int id, AddExpenseTypeDto dto)
		{
			if (id == 0)
				return BadRequest("Enter Valid ID");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var response = await expenseRepo.UpdateExpenseTypeAsync(id, dto);
			return Ok(response);
		}

		[HttpGet("~/GetExpensesForFarmRecord")]
		public async Task<IActionResult> GetExpensesForFarmRecord(int id)
		{
			if (id == 0)
				return BadRequest("Please enter valid record id");
			var response = await expenseRepo.GetExpensesForFarmRecord(id);
			return Ok(response);

		}

		[HttpGet("~/GetExpensesWithData")]
		public async Task<IActionResult> GetExpensesWithData(int id, int pageNumber = 1, int pageSize = 500)
		{
			if (id == 0)
				return BadRequest("There is no data for this id");
			var response = await expenseRepo.GetExpensesRecordsWithDataByExpenseId(id, pageNumber, pageSize);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseID = 0;
				response.ResponseMessage = "There is no records";
				return Ok(response);
			}
		}

		[HttpPost("~/PayToExpense")]
		public async Task<IActionResult> PayToExpense(ExpensePaymentDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await expenseRepo.PayToExpense(dto);
			return Ok(response);
		}

	}
}
