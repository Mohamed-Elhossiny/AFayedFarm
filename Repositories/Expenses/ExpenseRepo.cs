﻿using AFayedFarm.Dtos;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Repositories.Expenses
{
	public class ExpenseRepo : IExpenseRepo
	{
		private readonly FarmContext context;

		public ExpenseRepo(FarmContext context)
		{
			this.context = context;
		}

		public async Task<RequestResponse<ExpenseDto>> AddExpenseAsync(AddExpenseDto expenseDto)
		{
			var response = new RequestResponse<ExpenseDto>() { ResponseValue = new ExpenseDto() };
			var expensedb = context.Expenses.Include(c => c.ExpenseType).Where(f => f.ExpenseName.ToLower() == expenseDto.Name.ToLower()).FirstOrDefault();
			if (expensedb == null)
			{
				var Expense = new Expense()
				{
					ExpenseName = expenseDto.Name,
					ExpenseTypeId = expenseDto.ExpenseTypeId
				};
				await context.Expenses.AddAsync(Expense);
				await context.SaveChangesAsync();
				var expenseAdded = await context.Expenses.Include(c => c.ExpenseType).Where(c => c.ExpenseID == Expense.ExpenseID).FirstOrDefaultAsync();
				response.ResponseValue.ID = Expense.ExpenseID;
				response.ResponseValue.Name = Expense.ExpenseName;
				response.ResponseValue.ExpenseTypeName = expenseAdded.ExpenseTypeId != null ? Expense.ExpenseType.ExpenseTypeName : "";
				response.ResponseID = 1;
				return response;
			}
			response.ResponseID = 0;
			return response;
		}

		public async Task<RequestResponse<ExpenseRecordDto>> AddExpenseRecordAsync(AddExpenseRecordDto dto)
		{
			var response = new RequestResponse<ExpenseRecordDto> { ResponseID = 0, ResponseValue = new ExpenseRecordDto() };
			var expenseRecord = new ExpenseRecord();
			if (dto != null)
			{
				var remaining = (dto.Total - dto.Paied);
				expenseRecord.FarmRecordID = dto.FarmRecordID;
				expenseRecord.ExpenseID = dto.ExpenseID;
				expenseRecord.ExpenseDate = dto.ExpenseDate.Value != null ? dto.ExpenseDate.Value.Date : null;
				expenseRecord.Created_Date = dto.Created_Date.Value != null ? dto.Created_Date.Value.Date : null;
				expenseRecord.Quantity = dto.Quantity;
				expenseRecord.Value = dto.Value;
				expenseRecord.Price = dto.Price;
				expenseRecord.AdditionalPrice = dto.AdditionalPrice;
				expenseRecord.AdditionalNotes = dto.AdditionalNotes;
				expenseRecord.Total = dto.Total;
				expenseRecord.Paied = dto.Paied;
				expenseRecord.Remaining = remaining;
				expenseRecord.ExpenseNotes = dto.ExpenseRecordNotes;

				await context.ExpenseRecords.AddAsync(expenseRecord);
				await context.SaveChangesAsync();

				// TO DO
				// Get Expense Record
				var id = expenseRecord.ExpenseRecordId != 0 ? expenseRecord.ExpenseRecordId : 0;
				var expenseRecordDto = await GetExpenseRecordById(id);
				if (expenseRecordDto.ResponseID == 1)
				{
					response.ResponseValue = expenseRecordDto.ResponseValue;
					response.ResponseID = 1;
				}

			}
			return response;

		}

		public async Task<RequestResponse<ExpenseTypeDto>> AddExpenseTypeAsync(AddExpenseTypeDto dto)
		{
			var response = new RequestResponse<ExpenseTypeDto>() { ResponseID = 0, ResponseValue = new ExpenseTypeDto() };
			var expenseTypedb = context.TypeOfExpenses.Where(f => f.ExpenseTypeName.ToLower() == dto.ExpenseTypeName.ToLower()).FirstOrDefault();
			if (expenseTypedb == null)
			{
				var expenseType = new TypeOfExpense() { ExpenseTypeName = dto.ExpenseTypeName };
				await context.TypeOfExpenses.AddAsync(expenseType);
				await context.SaveChangesAsync();
				response.ResponseValue.ID = expenseType.ExpenseTypeID;
				response.ResponseValue.Name = expenseType.ExpenseTypeName;
				response.ResponseID = 1;
			}
			return response;
		}

		public async Task<RequestResponse<List<ExpenseTypeDto>>> GetAllExpenseTypes()
		{
			var response = new RequestResponse<List<ExpenseTypeDto>>() { ResponseID = 0 };
			var list = new List<ExpenseTypeDto>();
			var expenseTypeList = await context.TypeOfExpenses.ToListAsync();
			if (expenseTypeList.Count != 0)
			{
				foreach (var item in expenseTypeList)
				{
					var expenseTypeDto = new ExpenseTypeDto();
					expenseTypeDto.ID = item.ExpenseTypeID;
					expenseTypeDto.Name = item.ExpenseTypeName;
					list.Add(expenseTypeDto);
				}
				response.ResponseValue = list;
				response.ResponseID = 1;
			}
			return response;
		}

		public async Task<RequestResponse<List<ExpenseDto>>> GetExpenseAsync()
		{
			var response = new RequestResponse<List<ExpenseDto>>() { ResponseID = 0, ResponseValue = new List<ExpenseDto>() };
			var expensesDb = await context.Expenses.Include(x => x.ExpenseType).Select(c => new ExpenseDto
			{
				Name = c.ExpenseName,
				ID = c.ExpenseID,
				ExpenseTypeName = c.ExpenseTypeId != null ? c.ExpenseType.ExpenseTypeName : "",
			}).ToListAsync();

			if (expensesDb.Count != 0)
			{
				response.ResponseValue = expensesDb;
				response.ResponseID = 1;
			}
			return response;
		}

		public async Task<RequestResponse<ExpenseDto>> GetExpenseByID(int id)
		{
			var response = new RequestResponse<ExpenseDto>() { ResponseValue = new ExpenseDto() };
			var expenseDb = await context.Expenses.Include(c => c.ExpenseType).Where(e => e.ExpenseID == id).FirstOrDefaultAsync();
			if (expenseDb != null)
			{
				var Expense = new ExpenseDto()
				{
					Name = expenseDb.ExpenseName,
					ID = expenseDb.ExpenseID,
					ExpenseTypeName = expenseDb.ExpenseTypeId != null ? expenseDb.ExpenseType.ExpenseTypeName : ""
				};
				response.ResponseID = 1;
				response.ResponseValue = Expense;
				return response;
			}
			response.ResponseID = 0;
			return response;
		}

		public async Task<RequestResponse<ExpenseRecordDto>> GetExpenseRecordById(int id)
		{
			var response = new RequestResponse<ExpenseRecordDto> { ResponseID = 0, ResponseValue = new ExpenseRecordDto() };
			var expenseRecord = await context.ExpenseRecords
				.Include(c => c.FarmRecord).ThenInclude(c => c.Product)
				.Include(c => c.Expense)
				.Where(c => c.ExpenseRecordId == id).SingleOrDefaultAsync();
			var expesnseRecordDto = new ExpenseRecordDto();
			if (expenseRecord != null)
			{
				expesnseRecordDto.ExpenseRecordID = expenseRecord.ExpenseRecordId;
				expesnseRecordDto.FarmRecordID = expenseRecord.FarmRecordID;
				expesnseRecordDto.ExpenseID = expenseRecord.ExpenseID;
				expesnseRecordDto.ProductName = expenseRecord?.FarmRecord?.Product?.ProductName;
				expesnseRecordDto.ExpenseName = expenseRecord?.Expense?.ExpenseName;
				expesnseRecordDto.ExpenseDate = expenseRecord.ExpenseDate.Value != null ? expenseRecord.ExpenseDate.Value.Date : null;
				expesnseRecordDto.Created_Date = expenseRecord.Created_Date.Value != null ? expenseRecord.Created_Date.Value.Date : null;
				expesnseRecordDto.Quantity = expenseRecord.Quantity;
				expesnseRecordDto.Value = expenseRecord.Value;
				expesnseRecordDto.Price = expenseRecord.Price;
				expesnseRecordDto.AdditionalPrice = expenseRecord.AdditionalPrice;
				expesnseRecordDto.AdditionalNotes = expenseRecord.AdditionalNotes;
				expesnseRecordDto.Total = expenseRecord.Total;
				expesnseRecordDto.Paied = expenseRecord.Paied;
				expesnseRecordDto.Remaining = expenseRecord.Remaining;
				expesnseRecordDto.ExpenseRecordNotes = expenseRecord.ExpenseNotes;

				response.ResponseID = 1;
				response.ResponseValue = expesnseRecordDto;
			}
			return response;
		}

		public async Task<RequestResponse<ExpenseDto>> UpdateExpenseAsync(int id, AddExpenseDto expenseDto)
		{
			var response = new RequestResponse<ExpenseDto> { ResponseID = 0, ResponseValue = new ExpenseDto() };
			var expenseDb = await context.Expenses.Include(c => c.ExpenseType).SingleOrDefaultAsync(c => c.ExpenseID == id);
			if (expenseDb != null)
			{
				expenseDb.ExpenseName = expenseDto.Name;
				expenseDb.ExpenseTypeId = expenseDto.ExpenseTypeId;
				await context.SaveChangesAsync();
				var Expense = new ExpenseDto()
				{
					Name = expenseDb.ExpenseName,
					ID = expenseDb.ExpenseID,
					ExpenseTypeName = expenseDb.ExpenseTypeId != null ? expenseDb.ExpenseType.ExpenseTypeName : ""
				};
				response.ResponseID = 1;
				response.ResponseValue = Expense;
			}
			return response;
		}

		public async Task<RequestResponse<ExpenseRecordDto>> UpdateExpenseRecord(int id, AddExpenseRecordDto dto)
		{
			var response = new RequestResponse<ExpenseRecordDto> { ResponseID = 0, ResponseValue = new ExpenseRecordDto() };

			var expenseRecord = await context.ExpenseRecords
				.Include(c => c.FarmRecord).ThenInclude(c => c.Product)
				.Include(c => c.Expense)
				.Where(c => c.ExpenseRecordId == id).SingleOrDefaultAsync();
			if(expenseRecord != null)
			{
				var remaining = (dto.Total - dto.Paied);
				expenseRecord.FarmRecordID = dto.FarmRecordID;
				expenseRecord.ExpenseID = dto.ExpenseID;
				expenseRecord.ExpenseDate = dto.ExpenseDate.Value != null ? dto.ExpenseDate.Value.Date : null;
				expenseRecord.Created_Date = dto.Created_Date.Value != null ? dto.Created_Date.Value.Date : null;
				expenseRecord.Quantity = dto.Quantity;
				expenseRecord.Value = dto.Value;
				expenseRecord.Price = dto.Price;
				expenseRecord.AdditionalPrice = dto.AdditionalPrice;
				expenseRecord.AdditionalNotes = dto.AdditionalNotes;
				expenseRecord.Total = dto.Total;
				expenseRecord.Paied = dto.Paied;
				expenseRecord.Remaining = remaining;
				expenseRecord.ExpenseNotes = dto.ExpenseRecordNotes;

				context.ExpenseRecords.Update(expenseRecord);
				await context.SaveChangesAsync();

				var expenseFromGet = await GetExpenseRecordById(id);
				if(expenseFromGet.ResponseID == 1)
				{
					response.ResponseID = 1;
					response.ResponseValue = expenseFromGet.ResponseValue;
				}

			}
			return response;
		}
	}
}
