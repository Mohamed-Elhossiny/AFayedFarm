using AFayedFarm.Dtos;
using AFayedFarm.Enums;
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
			var expensedb = context.Expenses.Include(c => c.ExpenseType).Where(f => f.ExpenseName!.ToLower() == expenseDto.Name!.ToLower()).FirstOrDefault();
			if (expensedb == null)
			{
				var Expense = new Expense()
				{
					ExpenseName = expenseDto.Name,
					ExpenseTypeId = expenseDto.ExpenseTypeId,
					Create_Date = DateTime.Now.Date
				};
				await context.Expenses.AddAsync(Expense);
				await context.SaveChangesAsync();
				var expenseAdded = await context.Expenses.Include(c => c.ExpenseType).Where(c => c.ExpenseID == Expense.ExpenseID).FirstOrDefaultAsync();
				response.ResponseValue.ID = Expense.ExpenseID;
				response.ResponseValue.Name = Expense.ExpenseName;
				response.ResponseValue.ExpenseTypeName = expenseAdded!.ExpenseTypeId != 0 ? Expense.ExpenseType!.ExpenseTypeName : "";
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
				expenseRecord.ExpenseDate = dto.ExpenseDate!.Value.ToLongDateString() != "" ? dto.ExpenseDate.Value.Date : DateTime.Now.Date;
				expenseRecord.Created_Date = DateTime.Now.Date;
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

				#region Add Transactions to Financial Safe

				var transaction = new SafeTransaction();
				transaction.SafeID = 1;
				transaction.ExpenseID = dto.ExpenseID;
				transaction.TypeID = dto.TypeId;
				transaction.Type = ((TransactionType)dto.TypeId!).ToString();
				transaction.Total = -1 * dto.Paied;
				transaction.Notes = dto.ExpenseRecordNotes;

				await context.SafeTransactions.AddAsync(transaction);

				var financialSafe = await context.Safe.FindAsync(1);
				if (dto.TypeId == (int)TransactionType.Pay)
					financialSafe!.Total = financialSafe.Total - dto.Paied;

				context.Safe.Update(financialSafe!);

				var expenseDb = await context.Expenses.Where(f => f.ExpenseID == dto.ExpenseID).FirstOrDefaultAsync();
				if (expenseDb.TotalRemaining == null)
					expenseDb.TotalRemaining = 0;
				expenseDb!.TotalRemaining += remaining;
				context.Expenses.Update(expenseDb);

				await context.SaveChangesAsync();
				#endregion

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
			var expenseTypedb = context.TypeOfExpenses.Where(f => f.ExpenseTypeName!.ToLower() == dto.ExpenseTypeName!.ToLower()).FirstOrDefault();
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
			var expenseTypeList = await context.TypeOfExpenses.OrderByDescending(c => c.ExpenseTypeID).ToListAsync();
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

			var expenseListDto = new List<ExpenseDto>();
			var expensesDb = await context.Expenses.Include(x => x.ExpenseType).Select(c => new ExpenseDto
			{
				Name = c.ExpenseName,
				ID = c.ExpenseID,
				TotalRemaining = c.TotalRemaining,
				ExpenseTypeName = c.ExpenseTypeId != 0 ? c.ExpenseType!.ExpenseTypeName : "",
				Type = c.ExpenseTypeId != 0 ? c.ExpenseTypeId : 0,
			}).OrderByDescending(c => c.ID).ToListAsync();
			if (expensesDb.Count != 0)
			{
				foreach (var item in expensesDb)
				{
					if (item.TotalRemaining == null)
					{
						var remaning = await CalculateTotalRemainingFromRecords(item.ID);
						item.TotalRemaining = remaning.ResponseValue;
					}
					expenseListDto.Add(item);
				}

				response.ResponseValue = expenseListDto;
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
					ExpenseTypeName = expenseDb.ExpenseTypeId != 0 ? expenseDb.ExpenseType.ExpenseTypeName : "",
					Type = expenseDb.ExpenseTypeId,
					TotalRemaining = expenseDb.TotalRemaining == null ? 0 : expenseDb.TotalRemaining
				};
				if (expenseDb.TotalRemaining == null )
				{
					var reamining = await CalculateTotalRemainingFromRecords(id);
					Expense.TotalRemaining = reamining.ResponseValue;
				}
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
				.Include(c => c.Expense).ThenInclude(c => c.ExpenseType)
				.Where(c => c.ExpenseRecordId == id).SingleOrDefaultAsync();
			var expesnseRecordDto = new ExpenseRecordDto();
			if (expenseRecord != null)
			{
				expesnseRecordDto.ExpenseRecordID = expenseRecord.ExpenseRecordId;
				expesnseRecordDto.FarmRecordID = expenseRecord.FarmRecordID;
				expesnseRecordDto.ExpenseID = expenseRecord.ExpenseID;
				expesnseRecordDto.ProductName = expenseRecord?.FarmRecord?.Product?.ProductName;
				expesnseRecordDto.ExpenseName = expenseRecord?.Expense?.ExpenseName;
				expesnseRecordDto.ExpenseTypeName = expenseRecord?.Expense?.ExpenseType?.ExpenseTypeName;
				expesnseRecordDto.ExpenseDate = expenseRecord.ExpenseDate.HasValue ? expenseRecord.ExpenseDate.Value.Date : DateTime.Now.Date;
				expesnseRecordDto.Created_Date = expenseRecord.Created_Date.HasValue ? expenseRecord.Created_Date.Value.Date : DateTime.Now.Date;
				expesnseRecordDto.Quantity = expenseRecord.Quantity;
				expesnseRecordDto.Value = expenseRecord.Value;
				expesnseRecordDto.Price = expenseRecord.Price;
				expesnseRecordDto.AdditionalPrice = expenseRecord.AdditionalPrice;
				expesnseRecordDto.AdditionalNotes = expenseRecord.AdditionalNotes;
				expesnseRecordDto.Total = expenseRecord.Total;
				expesnseRecordDto.Paied = expenseRecord.Paied;
				expesnseRecordDto.Remaining = expenseRecord.Remaining;
				expesnseRecordDto.ExpenseRecordNotes = expenseRecord.ExpenseNotes;
				expesnseRecordDto.TypeId = 2;

				response.ResponseID = 1;
				response.ResponseValue = expesnseRecordDto;
			}
			return response;
		}

		public async Task<RequestResponse<ExpenseRecordsWithDataDto>> GetExpensesRecordsWithDataByExpenseId(int expenseId)
		{
			var response = new RequestResponse<ExpenseRecordsWithDataDto> { ResponseID = 0, ResponseValue = new ExpenseRecordsWithDataDto() };
			var expenseRecordList = await context.ExpenseRecords
				.Include(c => c.FarmRecord).ThenInclude(c => c.Product)
				.Include(c => c.Expense).ThenInclude(c => c.ExpenseType)
				.Where(c => c.ExpenseID == expenseId).OrderByDescending(c => c.ExpenseRecordId).ToListAsync();
			var transactionRecordDb = await context.SafeTransactions.Include(c => c.Expense).Where(c => c.ExpenseID == expenseId).ToListAsync();
			if (expenseRecordList.Count != 0 || transactionRecordDb.Count != 0)
			{
				var expesnseRecordListDto = new List<ExpenseRecordDto>();
				if (expenseRecordList.Count != 0)
				{
					foreach (var item in expenseRecordList)
					{
						var expesnseRecordDto = new ExpenseRecordDto();
						expesnseRecordDto.ExpenseRecordID = item.ExpenseRecordId;
						expesnseRecordDto.FarmRecordID = item.FarmRecordID;
						expesnseRecordDto.ExpenseID = item.ExpenseID;
						expesnseRecordDto.ProductName = item?.FarmRecord?.Product?.ProductName;
						expesnseRecordDto.ExpenseName = item?.Expense?.ExpenseName;
						expesnseRecordDto.ExpenseTypeName = item?.Expense?.ExpenseType.ExpenseTypeName;
						expesnseRecordDto.ExpenseDate = item.ExpenseDate.HasValue ? item.ExpenseDate.Value.Date : DateTime.Now.Date;
						expesnseRecordDto.Created_Date = item.Created_Date.HasValue ? item.Created_Date.Value.Date : DateTime.Now.Date;
						expesnseRecordDto.Quantity = item.Quantity;
						expesnseRecordDto.Value = item.Value;
						expesnseRecordDto.Price = item.Price;
						expesnseRecordDto.AdditionalPrice = item.AdditionalPrice;
						expesnseRecordDto.AdditionalNotes = item.AdditionalNotes;
						expesnseRecordDto.Total = item.Total;
						expesnseRecordDto.Paied = item.Paied;
						expesnseRecordDto.Remaining = item.Remaining;
						expesnseRecordDto.ExpenseRecordNotes = item.ExpenseNotes;

						expesnseRecordListDto.Add(expesnseRecordDto);
					}
				}
				if (transactionRecordDb.Count != 0)
				{
					foreach (var item in transactionRecordDb)
					{
						var transactionRecord = new ExpenseRecordDto();
						transactionRecord.ExpenseRecordID = item.ID;
						transactionRecord.Created_Date = item.Created_Date;
						transactionRecord.ExpenseID = (int)item.Expense!.ExpenseID;
						transactionRecord.ExpenseName = item.Expense!.ExpenseName;
						transactionRecord.Description = TransactionType.Pay.ToString();
						transactionRecord.Paied = -1 * item.Total;
						transactionRecord.ExpenseRecordNotes = item.Notes;

						expesnseRecordListDto.Add(transactionRecord);
					}
				}

				response.ResponseID = 1;
				response.ResponseValue.ExpensesList = expesnseRecordListDto;
			}
			var expnesesData = await GetExpenseByID(expenseId);

			response.ResponseValue.Name = expnesesData?.ResponseValue?.Name;
			response.ResponseValue.ID = expnesesData?.ResponseValue?.ID;
			response.ResponseValue.Total = expnesesData?.ResponseValue?.TotalRemaining;
			response.ResponseValue.ExpenseTypeName = expnesesData?.ResponseValue?.ExpenseTypeName;
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

				var Expnese = await GetExpenseByID(id);

				response.ResponseID = 1;
				response.ResponseValue = Expnese.ResponseValue;
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
			if (expenseRecord != null)
			{
				var remaining = (dto.Total - dto.Paied);
				expenseRecord.FarmRecordID = dto.FarmRecordID;
				expenseRecord.ExpenseID = dto.ExpenseID;
				expenseRecord.ExpenseDate = dto.ExpenseDate.HasValue ? dto.ExpenseDate.Value.Date : DateTime.Now.Date;
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

				#region Add Transactions to Financial Safe

				var transaction = new SafeTransaction();
				transaction.SafeID = 1;
				transaction.ExpenseID = dto.ExpenseID;
				transaction.TypeID = dto.TypeId;
				transaction.Type = ((TransactionType)dto.TypeId!).ToString();
				transaction.Total = -1 * dto.Paied;
				transaction.Notes = dto.ExpenseRecordNotes;

				await context.SafeTransactions.AddAsync(transaction);

				var financialSafe = await context.Safe.FindAsync(1);
				if (dto.TypeId == (int)TransactionType.Pay)
					financialSafe!.Total = financialSafe.Total - dto.Paied;

				context.Safe.Update(financialSafe!);

				var expenseDb = await context.Expenses.Where(f => f.ExpenseID == dto.ExpenseID).FirstOrDefaultAsync();
				if (expenseDb!.TotalRemaining == null)
					expenseDb.TotalRemaining = 0;
				if (dto.TypeId == (int)TransactionType.Pay)
				{
					expenseDb!.TotalRemaining = expenseDb.TotalRemaining - dto.Paied;
				}
				context.Expenses.Update(expenseDb);

				await context.SaveChangesAsync();
				#endregion

				var expenseFromGet = await GetExpenseRecordById(id);
				if (expenseFromGet.ResponseID == 1)
				{
					response.ResponseID = 1;
					response.ResponseValue = expenseFromGet.ResponseValue;
				}

			}
			return response;
		}

		public async Task<RequestResponse<decimal>> CalculateTotalRemainingFromRecords(int expenseID)
		{
			var response = new RequestResponse<decimal> { ResponseID = 0, ResponseValue = 0 };
			var expenseDb = await context.Expenses.Where(c => c.ExpenseID == expenseID).FirstOrDefaultAsync();
			var expenseList = await GetAllExpenseRecordsByExpenseId(expenseID);
			if (expenseList.ResponseID == 1)
			{
				decimal total = 0;
				foreach (var item in expenseList.ResponseValue!)
				{
					total += (decimal)item.Remaining;
				}
				response.ResponseID = 1;
				response.ResponseValue = (decimal)total!;

				expenseDb!.TotalRemaining = total;
				context.Expenses.Update(expenseDb);
				await context.SaveChangesAsync();
				return response;
			}
			expenseDb!.TotalRemaining = 0;
			context.Expenses.Update(expenseDb);
			await context.SaveChangesAsync();
			return response;

		}

		public async Task<RequestResponse<List<ExpenseRecordDto>>> GetExpensesForFarmRecord(int farmRecordID)
		{
			var response = new RequestResponse<List<ExpenseRecordDto>> { ResponseID = 0, ResponseValue = new List<ExpenseRecordDto>() };
			var expenseRecordList = await context.ExpenseRecords
				.Include(c => c.FarmRecord).ThenInclude(c => c.Product)
				.Include(c => c.Expense).ThenInclude(c => c.ExpenseType)
				.Where(c => c.FarmRecordID == farmRecordID).OrderByDescending(c => c.ExpenseRecordId).ToListAsync();
			if (expenseRecordList.Count != 0)
			{
				var expesnseRecordListDto = new List<ExpenseRecordDto>();
				foreach (var item in expenseRecordList)
				{
					var expesnseRecordDto = new ExpenseRecordDto();
					expesnseRecordDto.ExpenseRecordID = item.ExpenseRecordId;
					expesnseRecordDto.FarmRecordID = item.FarmRecordID;
					expesnseRecordDto.ExpenseID = item.ExpenseID;
					expesnseRecordDto.ProductName = item?.FarmRecord?.Product?.ProductName;
					expesnseRecordDto.ExpenseName = item?.Expense?.ExpenseName;
					expesnseRecordDto.ExpenseTypeName = item?.Expense?.ExpenseType?.ExpenseTypeName;
					expesnseRecordDto.ExpenseDate = item.ExpenseDate.HasValue ? item.ExpenseDate.Value.Date : DateTime.Now.Date;
					expesnseRecordDto.Created_Date = item.Created_Date.HasValue ? item.Created_Date.Value.Date : DateTime.Now.Date;
					expesnseRecordDto.Quantity = item.Quantity;
					expesnseRecordDto.Value = item.Value;
					expesnseRecordDto.Price = item.Price;
					expesnseRecordDto.AdditionalPrice = item.AdditionalPrice;
					expesnseRecordDto.AdditionalNotes = item.AdditionalNotes;
					expesnseRecordDto.Total = item.Total;
					expesnseRecordDto.Paied = item.Paied;
					expesnseRecordDto.Remaining = item.Remaining;
					expesnseRecordDto.ExpenseRecordNotes = item.ExpenseNotes;

					expesnseRecordListDto.Add(expesnseRecordDto);
				}

				response.ResponseID = 1;
				response.ResponseValue = expesnseRecordListDto;
			}
			return response;

		}

		public async Task<RequestResponse<List<ExpenseRecordDto>>> GetAllExpenseRecordsByExpenseId(int expenseID)
		{
			var response = new RequestResponse<List<ExpenseRecordDto>> { ResponseID = 0, ResponseValue = new List<ExpenseRecordDto>() };
			var expesnseRecordListDto = new List<ExpenseRecordDto>();
			var expenseRecordList = await context.ExpenseRecords
				.Include(c => c.FarmRecord).ThenInclude(c => c.Product)
				.Include(c => c.Expense).ThenInclude(c => c.ExpenseType).OrderByDescending(c => c.ExpenseRecordId)
				.Where(c => c.ExpenseID == expenseID).ToListAsync();
			var transactionRecordDb = await context.SafeTransactions.Include(c => c.Expense).Where(c => c.ExpenseID == expenseID).ToListAsync();

			if (expenseRecordList.Count != 0 || transactionRecordDb.Count != 0)
			{
				if (expenseRecordList.Count != 0)
				{
					foreach (var item in expenseRecordList)
					{
						var expesnseRecordDto = new ExpenseRecordDto();
						expesnseRecordDto.ExpenseRecordID = item.ExpenseRecordId;
						expesnseRecordDto.FarmRecordID = item.FarmRecordID;
						expesnseRecordDto.ExpenseID = item.ExpenseID;
						expesnseRecordDto.ProductName = item?.FarmRecord?.Product?.ProductName;
						expesnseRecordDto.ExpenseName = item?.Expense?.ExpenseName;
						expesnseRecordDto.ExpenseTypeName = item?.Expense?.ExpenseType.ExpenseTypeName;
						expesnseRecordDto.ExpenseDate = item.ExpenseDate.HasValue ? item.ExpenseDate.Value.Date : DateTime.Now.Date;
						expesnseRecordDto.Created_Date = item.Created_Date.HasValue ? item.Created_Date.Value.Date : DateTime.Now.Date;
						expesnseRecordDto.Quantity = item.Quantity;
						expesnseRecordDto.Value = item.Value;
						expesnseRecordDto.Price = item.Price;
						expesnseRecordDto.AdditionalPrice = item.AdditionalPrice;
						expesnseRecordDto.AdditionalNotes = item.AdditionalNotes;
						expesnseRecordDto.Total = item.Total;
						expesnseRecordDto.Paied = item.Paied;
						expesnseRecordDto.Remaining = item.Remaining;
						expesnseRecordDto.ExpenseRecordNotes = item.ExpenseNotes;

						expesnseRecordListDto.Add(expesnseRecordDto);
					}
				}
				if (transactionRecordDb.Count != 0)
				{
					foreach (var item in transactionRecordDb)
					{
						var transactionRecord = new ExpenseRecordDto();
						transactionRecord.ExpenseRecordID = item.ID;
						transactionRecord.ExpenseID = (int)item.Expense!.ExpenseID;
						transactionRecord.ExpenseName = item.Expense!.ExpenseName;
						transactionRecord.Description = TransactionType.Pay.ToString();
						transactionRecord.Paied = -1 * item.Total;
						transactionRecord.ExpenseRecordNotes = item.Notes;
						transactionRecord.Created_Date = item.Created_Date;

						expesnseRecordListDto.Add(transactionRecord);
					}
				}

				response.ResponseID = 1;
				response.ResponseValue = expesnseRecordListDto;
			}

			return response;
		}

		public async Task<RequestResponse<ExpenseDto>> PayToExpense(ExpensePaymentDto dto)
		{
			var response = new RequestResponse<ExpenseDto> { ResponseID = 0, ResponseValue = new ExpenseDto() };
			var expenseDb = await context.Expenses.Where(e => e.ExpenseID == dto.Id).FirstOrDefaultAsync();
			if (expenseDb == null)
				return response;

			#region Add Transactions to Financial Safe
			var transaction = new SafeTransaction()
			{
				SafeID = 1,
				ExpenseID = dto.Id,
				TypeID = dto.TrasactionTypeID,
				Type = ((TransactionType)dto.TrasactionTypeID!).ToString(),
				Total = -1 * dto.Total,
				Notes = dto.Notes,
				Created_Date = DateTime.Now.Date
			};
			await context.SafeTransactions.AddAsync(transaction);

			var financialSafe = await context.Safe.FindAsync(1);
			if (dto.TrasactionTypeID == (int)TransactionType.Pay)
				financialSafe!.Total = financialSafe.Total - dto.Total;
			context.Safe.Update(financialSafe!);

			// Subtract Pay Amount from Total Remaining of Fram
			expenseDb.TotalRemaining -= dto.Total;
			context.Expenses.Update(expenseDb);

			await context.SaveChangesAsync();
			#endregion
			var expense = await GetExpenseByID((int)dto.Id!);
			response.ResponseID = 1;
			response.ResponseValue = expense.ResponseValue;
			return response;
		}
	}
}
