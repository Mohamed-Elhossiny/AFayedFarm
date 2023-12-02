﻿using AFayedFarm.Dtos;
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
		[HttpPost]
		public async Task<IActionResult> AddExpenseAsync(AddExpenseDto expenseDto)
		{
			if (expenseDto.ExpenseName == null || expenseDto.ExpenseName == "")
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

		[HttpGet]
		public async Task<IActionResult> GetAllExpenses()
		{
			var response = await expenseRepo.GetExpenseAsync();
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			return NotFound("No Data Found");
		}

		[HttpGet("id:int")]
		public async Task<IActionResult> GetExpenseById(int id)
		{
			var response = await expenseRepo.GetExpenseByID(id);
			if (response.ResponseID == 0)
				return NotFound($"No Exepnse Found By ID {id}");
			return Ok(response.ResponseValue);
		}

		[HttpPut("id:int")]
		public async Task<IActionResult> UpdateExpense(int id, [FromBody] AddExpenseDto expenseDto)
		{
			var response = await expenseRepo.GetExpenseByID(id);
			if (response.ResponseID == 0)
				return NotFound($"No expense found by this {id}");
			if (expenseDto.ExpenseName == "")
				return BadRequest("Please enter expense name");

			var requestResponse = await expenseRepo.UpdateExpenseAsync(id, expenseDto);
			if (requestResponse.ResponseID == 1)
				return Ok(requestResponse.ResponseValue);
			return Conflict("Can't Updated Expense");
		}
	}
}
