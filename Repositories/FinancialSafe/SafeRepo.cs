using AFayedFarm.Controllers;
using AFayedFarm.Dtos;
using AFayedFarm.Dtos.Financial;
using AFayedFarm.Enums;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AFayedFarm.Repositories.FinancialSafe
{
	public class SafeRepo : ISafeRepo
	{
		private readonly FarmContext context;

		public SafeRepo(FarmContext context)
		{
			this.context = context;
		}

		public async Task<RequestResponse<SafeDto>> AddBalance(BalanceDto dto)
		{
			var response = new RequestResponse<SafeDto> { ResponseID = 0, ResponseValue = new SafeDto() };
			var safeDb = await context.Safe.FirstOrDefaultAsync();
			if (safeDb != null)
			{
				var safeTransaction = new SafeTransaction();
				if (dto.typeId == 4)
				{
					safeDb.Total += dto.Balance;
					context.Safe.Update(safeDb);
					await context.SaveChangesAsync();

					safeTransaction.Total =  dto.Balance;
					safeTransaction.Notes = "إيداع في الخزنة";
					safeTransaction.TypeID = (int)TransactionType.Income;
					safeTransaction.Type = TransactionType.Income.ToString();
					safeTransaction.Created_Date = DateTime.Now;

					await context.SafeTransactions.AddAsync(safeTransaction);
					await context.SaveChangesAsync();
				}

				response.ResponseID = 1;
				response.ResponseValue.ID = safeDb.ID;
				response.ResponseValue.Total = safeDb.Total;
			}
			return response;
		}

		public async Task<RequestResponse<SafeDto>> Withdraw(WithdrawDto dto)
		{
			var response = new RequestResponse<SafeDto> { ResponseID = 0,ResponseValue = new SafeDto() };
			var safe = await context.Safe.FindAsync(2);
			if (safe != null)
			{
				var safeTransaction = new SafeTransaction();

				if (dto.Total != null)
				{
					safe.Total -= dto.Total;
					context.Safe.Update(safe);

					safeTransaction.Total = -1 * dto.Total;
					safeTransaction.Notes = dto.Notes != null ? dto.Notes : "";
					safeTransaction.TypeID = (int)TransactionType.Admin_Expense;
					safeTransaction.Type = TransactionType.Admin_Expense.ToString();
					safeTransaction.Created_Date = DateTime.Now;

					await context.SafeTransactions.AddAsync(safeTransaction);
					await context.SaveChangesAsync();

					response.ResponseID = 1;
					response.ResponseValue!.ID = safe.ID;
					response.ResponseValue!.Total = safe.Total;
					return response;
				}
			}
			return response;
		}

		public async Task<RequestResponse<List<AllFinancialRecordsDto>>> GetAllFinancialRecords(int currentPage = 1, int pageSize = 100, DateTime? from = null, DateTime? to = null)
		{
			var response = new RequestResponse<List<AllFinancialRecordsDto>>() { ResponseID = 0, ResponseValue = new List<AllFinancialRecordsDto>() };

			var query = context.SafeTransactions
				.Include(c => c.Client)
				.Include(c => c.Employee)
				.Include(c => c.Farm)
				.Include(c => c.Expense)
				.Include(c => c.Fridge)
				.OrderByDescending(c => c.Created_Date);

			if (from.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date >= from.Value);
			}
			if (to.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date <= to.Value);
			}

			var allLists = await query.ToListAsync();

			var allList = allLists.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (allList.Count != 0)
			{
				//int index = 1;
				var allListDto = new List<AllFinancialRecordsDto>();
				foreach (var item in allList)
				{
					var list = new AllFinancialRecordsDto();

					//list.index = index;
					list.ID = item.ID;
					list.SafeID = item.SafeID;
					list.Total = item.Total;
					list.Date = item.Created_Date;
					list.Notes = item.Notes;
					list.Type = item.Type;
					list.TypeID = item.TypeID;

					list.ClientID = item.CLientID;
					list.ClientName = item.Client?.ClientName ?? "";

					list.EmpID = item.Emp_ID;
					list.EmpName = item.Employee?.Full_Name ?? "";

					list.ExpenseID = item.ExpenseID;
					list.ExpenseName = item.Expense?.ExpenseName ?? "";

					list.FarmID = item.FarmID;
					list.FarmName = item.Farm?.FarmsName ?? "";

					list.FridgeID = item.FridgeID;
					list.FridgeName = item.Fridge?.FridgeName ?? "";
					//++index;

					allListDto.Add(list);
				}

				response.PageSize = pageSize;
				response.LastPage = (int)Math.Ceiling((double)allLists.Count() / pageSize);
				response.CurrentPage = currentPage;
				response.ResponseID = 1;
				response.ResponseValue = allListDto;
			}

			return response;
		}

		public async Task<RequestResponse<List<FinancialClientDto>>> GetClientFinancialRecords(int currentPage = 1, int pageSize = 100, DateTime? from = null, DateTime? to = null)
		{
			var response = new RequestResponse<List<FinancialClientDto>>() { ResponseID = 0, ResponseValue = new List<FinancialClientDto>() };

			var query = context.SafeTransactions
				.Include(c => c.Client)
				.Where(c => c.CLientID != null)
				.OrderByDescending(c => c.Created_Date);

			if (from.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date >= from.Value);
			}
			if (to.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date <= to.Value);
			}
			var clientLists = await query.ToListAsync();

			var clientList = clientLists.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (clientList.Count != 0)
			{
				//int index = 1;
				var clientListDto = new List<FinancialClientDto>();
				foreach (var item in clientList)
				{
					var client = new FinancialClientDto();

					//client.index = index;
					client.ID = item.ID;
					client.SafeID = item.SafeID;
					client.Total = item.Total;
					client.Date = item.Created_Date;
					client.Notes = item.Notes;
					client.Type = item.Type;
					client.TypeID = item.TypeID;
					client.ClientID = item.CLientID;
					client.ClientName = item.Client?.ClientName ?? "";
					//++index;

					clientListDto.Add(client);
				}
				response.ResponseID = 1;
				response.ResponseValue = clientListDto;
				response.PageSize = pageSize;
				response.LastPage = (int)Math.Ceiling((double)clientLists.Count() / pageSize);
				response.CurrentPage = currentPage;
			}

			return response;
		}

		public async Task<RequestResponse<List<FinancialEmployeeDto>>> GetEmployeeFinancialRecords(int currentPage = 1, int pageSize = 100, DateTime? from = null, DateTime? to = null)
		{
			var response = new RequestResponse<List<FinancialEmployeeDto>>() { ResponseID = 0, ResponseValue = new List<FinancialEmployeeDto>() };
			var query = context.SafeTransactions
				.Include(c => c.Employee)
				.Where(c => c.Emp_ID != null)
				.OrderByDescending(c => c.Created_Date);

			if (from.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date >= from.Value);
			}
			if (to.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date <= to.Value);
			}

			var employeeLists = await query.ToListAsync();

			var employeeList = employeeLists.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (employeeList.Count != 0)
			{
				//int index = 1;
				var empListDto = new List<FinancialEmployeeDto>();
				foreach (var item in employeeList)
				{
					var emp = new FinancialEmployeeDto();

					//emp.index = index;
					emp.ID = item.ID;
					emp.SafeID = item.SafeID;
					emp.Total = item.Total;
					emp.Date = item.Created_Date;
					emp.Notes = item.Notes;
					emp.Type = item.Type;
					emp.TypeID = item.TypeID;
					emp.EmpID = item.Emp_ID;
					emp.EmpName = item.Employee?.Full_Name ?? "";
					//++index;

					empListDto.Add(emp);
				}
				response.ResponseID = 1;
				response.ResponseValue = empListDto;
				response.PageSize = pageSize;
				response.LastPage = (int)Math.Ceiling((double)employeeLists.Count() / pageSize);
				response.CurrentPage = currentPage;
			}

			return response;
		}

		public async Task<RequestResponse<List<FinancialExpenseDto>>> GetExpenseFinancialRecords(int currentPage = 1, int pageSize = 100, DateTime? from = null, DateTime? to = null)
		{
			var response = new RequestResponse<List<FinancialExpenseDto>>() { ResponseID = 0, ResponseValue = new List<FinancialExpenseDto>() };
			var query = context.SafeTransactions
				.Include(c => c.Expense)
				.Where(c => c.ExpenseID != null)
				.OrderByDescending(c => c.Created_Date);

			if (from.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date >= from.Value);
			}
			if (to.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date <= to.Value);
			}
			var expenseLists = await query.ToListAsync();
			//var expenseList = await expenseLists.ToListAsync();
			var expenseList = expenseLists.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (expenseList.Count != 0)
			{
				//var index = 1;
				var expenseListDto = new List<FinancialExpenseDto>();
				foreach (var item in expenseList)
				{
					var expense = new FinancialExpenseDto();

					//expense.index = index;
					expense.ID = item.ID;
					expense.SafeID = item.SafeID;
					expense.Total = item.Total;
					expense.Date = item.Created_Date;
					expense.Notes = item.Notes;
					expense.Type = item.Type;
					expense.TypeID = item.TypeID;
					expense.ExpenseID = item.ExpenseID;
					expense.ExpenseName = item.Expense?.ExpenseName ?? "";
					//++index;

					expenseListDto.Add(expense);
				}
				response.ResponseID = 1;
				response.ResponseValue = expenseListDto;
				response.PageSize = pageSize;
				response.LastPage = (int)Math.Ceiling((double)expenseLists.Count() / pageSize);
				response.CurrentPage = currentPage;
			}

			return response;
		}

		public async Task<RequestResponse<List<FinancialFarmDto>>> GetFarmFinancialRecords(int currentPage = 1, int pageSize = 100, DateTime? from = null, DateTime? to = null)
		{
			var response = new RequestResponse<List<FinancialFarmDto>>() { ResponseID = 0, ResponseValue = new List<FinancialFarmDto>() };
			var query = context.SafeTransactions.Include(c => c.Farm)
				.Where(c => c.FarmID != null).OrderByDescending(c => c.Created_Date);

			if (from.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date >= from.Value);
			}
			if (to.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date <= to.Value);
			}

			var farmLists = await query.ToListAsync();

			var farmList = farmLists.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (farmList.Count != 0)
			{
				//var index = 1;
				var farmListDto = new List<FinancialFarmDto>();
				foreach (var item in farmList)
				{
					var farm = new FinancialFarmDto();

					//farm.index = index;
					farm.ID = item.ID;
					farm.SafeID = item.SafeID;
					farm.Total = item.Total;
					farm.Date = item.Created_Date;
					farm.Notes = item.Notes;
					farm.Type = item.Type;
					farm.TypeID = item.TypeID;
					farm.FarmID = item.FarmID;
					farm.FarmName = item.Farm?.FarmsName ?? "";
					//++index;

					farmListDto.Add(farm);
				}
				response.ResponseID = 1;
				response.ResponseValue = farmListDto;
				response.PageSize = pageSize;
				response.LastPage = (int)Math.Ceiling((double)farmLists.Count() / pageSize);
				response.CurrentPage = currentPage;
			}

			return response;
		}

		public async Task<RequestResponse<List<FinancialFridgeDto>>> GetFridgeFinancialRecords(int currentPage = 1, int pageSize = 100, DateTime? from = null, DateTime? to = null)
		{
			var response = new RequestResponse<List<FinancialFridgeDto>>() { ResponseID = 0, ResponseValue = new List<FinancialFridgeDto>() };
			var query = context.SafeTransactions
				.Include(c => c.Fridge)
				.Where(c => c.FridgeID != null)
				.OrderByDescending(c => c.Created_Date);

			if (from.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date >= from.Value);
			}
			if (to.HasValue)
			{
				query = (IOrderedQueryable<SafeTransaction>)query.Where(c => c.Created_Date <= to.Value);
			}

			var fridgeLists = await query.ToListAsync();


			var fridgeList = fridgeLists.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (fridgeList.Count != 0)
			{
				//var index = 1;
				var fridgeListDto = new List<FinancialFridgeDto>();
				foreach (var item in fridgeList)
				{
					var fridge = new FinancialFridgeDto();
					//fridge.index = index;
					fridge.ID = item.ID;
					fridge.SafeID = item.SafeID;
					fridge.Total = item.Total;
					fridge.Date = item.Created_Date;
					fridge.Notes = item.Notes;
					fridge.Type = item.Type;
					fridge.TypeID = item.TypeID;
					fridge.FridgeID = item.FridgeID;
					fridge.FridgeName = item.Fridge?.FridgeName ?? "";
					//++index;

					fridgeListDto.Add(fridge);
				}
				response.ResponseID = 1;
				response.ResponseValue = fridgeListDto;
				response.PageSize = pageSize;
				response.LastPage = (int)Math.Ceiling((double)fridgeLists.Count() / pageSize);
				response.CurrentPage = currentPage;
			}

			return response;
		}

		public async Task<RequestResponse<SafeDto>> GetSafeBalance()
		{
			var response = new RequestResponse<SafeDto> { ResponseID = 0, ResponseValue = new SafeDto() };
			var safeDb = await context.Safe.FirstOrDefaultAsync();
			if (safeDb != null)
			{
				var safe = new SafeDto
				{
					ID = safeDb.ID,
					Total = safeDb.Total,
				};

				response.ResponseValue = safe;
				response.ResponseID = 1;
			}
			return response;
		}

	}
}
