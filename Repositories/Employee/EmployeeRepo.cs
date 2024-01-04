using AFayedFarm.Dtos;
using AFayedFarm.Enums;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AFayedFarm.Repositories.Employee
{
	public class EmployeeRepo : IEmployeeRepo
	{
		private const string ConfigFilePath = "config.json";
		private class ConfigModel
		{
			public bool IsPayed { get; set; }
			public int LastProcessedMonth { get; set; }
		}
		private readonly FarmContext context;
		public EmployeeRepo(FarmContext context)
		{
			this.context = context;
		}
		public async Task<RequestResponse<EmployeeDto>> AddEmployee(AddEmployeeDto dto)
		{
			var response = new RequestResponse<EmployeeDto> { ResponseID = 0 };
			var empolyee = new EmployeeDto();
			var empdb = context.Employees.Where(f => f.Full_Name!.ToLower() == dto.Name!.ToLower()).FirstOrDefault();
			if (empdb == null)
			{
				var emp = new Model.Employee()
				{
					Full_Name = dto.Name,
					Salary = dto.Salary,
					Create_Date = DateTime.Now,
					TotalBalance = 0,
					//TestDate = DateTime.Now
				};
				await context.Employees.AddAsync(emp);
				await context.SaveChangesAsync();
				empolyee.Name = emp.Full_Name;
				empolyee.ID = emp.EmpolyeeID;
				empolyee.Salary = emp.Salary;
				empolyee.Total = emp.TotalBalance;
				empolyee.Created_Date = DateOnly.FromDateTime(emp.Create_Date ?? DateTime.Now);

				response.ResponseID = 1;
				response.ResponseValue = empolyee;
				return response;
			}
			return response;
		}

		public async Task<RequestResponse<List<EmployeeDto>>> GetAllEmployee()
		{
			#region Check Monthly Salaries
			var today = DateTime.UtcNow.Date;
			var currentMonth = today.Month;
			var config = LoadConfig();
			if (!config.IsPayed && currentMonth != config.LastProcessedMonth)
			{
				var firstDayOfMonth = new DateTime(today.Year, today.Month, 1).Date;
				if (today == firstDayOfMonth)
				{
					var salariesAdded = await PayMonlthySalary();
					config.IsPayed = true;
					config.LastProcessedMonth = currentMonth;
					SaveConfig(config);
				}
			}
			else if (currentMonth != config.LastProcessedMonth) //2 != 1
			{
				config.IsPayed = false;
				SaveConfig(config);
			}
			#endregion

			var response = new RequestResponse<List<EmployeeDto>> { ResponseID = 0, ResponseValue = new List<EmployeeDto>() };
			var empsDb = await context.Employees.Select(f => new EmployeeDto
			{
				ID = f.EmpolyeeID,
				Name = f.Full_Name,
				Salary = f.Salary,
				Total = f.TotalBalance != null ? f.TotalBalance : 0,
				Created_Date = DateOnly.FromDateTime(f.Create_Date ?? DateTime.Now),
			}).OrderByDescending(c => c.ID).ToListAsync();

			if (empsDb.Count != 0)
			{
				response.ResponseID = 1;
				response.ResponseValue = empsDb;
			}

			return response;
		}

		public async Task<RequestResponse<EmployeeDto>> GetEmployee(int empId)
		{
			var response = new RequestResponse<EmployeeDto> { ResponseID = 0 };
			var empDb = await context.Employees.Where(c => c.EmpolyeeID == empId).FirstOrDefaultAsync();
			if (empDb != null)
			{
				var empolyee = new EmployeeDto();
				empolyee.Name = empDb.Full_Name;
				empolyee.ID = empDb.EmpolyeeID;
				empolyee.Salary = empDb.Salary;
				empolyee.Total = empDb.TotalBalance != null ? empDb.TotalBalance : 0;
				empolyee.Created_Date = DateOnly.FromDateTime(empDb.Create_Date ?? DateTime.Now);

				response.ResponseID = 1;
				response.ResponseValue = empolyee;
				return response;
			}
			return response;
		}

		public async Task<RequestResponse<EmployeeDto>> UpdateEmployee(int empId, AddEmployeeDto dto)
		{
			var response = new RequestResponse<EmployeeDto> { ResponseID = 0, ResponseValue = new EmployeeDto() };

			var empDb = await context.Employees.SingleOrDefaultAsync(m => m.EmpolyeeID == empId);
			if (empDb != null)
			{
				empDb.Full_Name = dto.Name;
				empDb.Salary = dto.Salary;

				context.Employees.Update(empDb);
				await context.SaveChangesAsync();

				response.ResponseID = 1;
				response.ResponseValue.Salary = empDb.Salary;
				response.ResponseValue.Name = empDb.Full_Name;
				response.ResponseValue.ID = empDb.EmpolyeeID;
				response.ResponseValue.Created_Date = DateOnly.FromDateTime(empDb.Create_Date ?? DateTime.Now);

			}

			return response;

		}

		public async Task<RequestResponse<EmployeeDto>> PayToEmployee(EmployeePaymentDto dto)
		{
			var response = new RequestResponse<EmployeeDto> { ResponseID = 0 };
			try
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					var empDb = await context.Employees.Where(e => e.EmpolyeeID == dto.Id).FirstOrDefaultAsync();
					if (empDb == null)
						return response;

					#region Subtract Pay Amount from Total of Employee
					if (dto.TrasactionTypeID == (int)TransactionType.Pay)
					{
						empDb.TotalBalance -= dto.Total;
						context.Employees.Update(empDb);
					}
					#endregion

					#region Add Transactions to Financial Safe
					var Safetransaction = new SafeTransaction()
					{
						SafeID = 1,
						Emp_ID = dto.Id,
						TypeID = dto.TrasactionTypeID,
						Total = -1 * dto.Total,
						Type = ((TransactionType)dto.TrasactionTypeID!).ToString(),
						Notes = dto.Notes,
						Created_Date = DateTime.Now.Date
					};
					await context.SafeTransactions.AddAsync(Safetransaction);

					var financialSafe = await context.Safe.FindAsync(1);
					if (dto.TrasactionTypeID == (int)TransactionType.Pay)
						financialSafe!.Total = financialSafe.Total - dto.Total;
					context.Safe.Update(financialSafe!);

					await context.SaveChangesAsync();
					transaction.Commit();
				}
			}
			catch (Exception ex)
			{
				if (context.Database.CurrentTransaction != null)
					context.Database.CurrentTransaction.Rollback();
			}
			#endregion
			var employee = await GetEmployee((int)dto.Id!);
			if (employee.ResponseID == 1)
			{
				response.ResponseID = 1;
				response.ResponseValue = employee.ResponseValue;
			}
			else
			{
				response.ResponseID = 0;
			}
			return response;
		}

		public async Task<RequestResponse<bool>> PayMonlthySalary()
		{
			var response = new RequestResponse<bool>() { ResponseID = 0, ResponseValue = false };
			var empsDb = await context.Employees.ToListAsync();
			if (empsDb.Count != 0)
			{
				foreach (var item in empsDb)
				{
					if (item.TotalBalance == null)
						item.TotalBalance = 0;
					item.TotalBalance += item.Salary;
					context.Employees.Update(item);
					await context.SaveChangesAsync();

					#region Add Transactions to Financial Safe
					var financialSafe = await context.Safe.FindAsync(1);
					var transaction = new SafeTransaction()
					{
						SafeID = 1,
						Emp_ID = item.EmpolyeeID,
						TypeID = (int)TransactionType.MonthlySalary,
						Type = TransactionType.MonthlySalary.ToString(),
						Total = -1 * item.Salary,
						Created_Date = DateTime.Now.Date
					};
					await context.SafeTransactions.AddAsync(transaction);

					financialSafe.Total = financialSafe.Total - item.Salary;
					context.Safe.Update(financialSafe!);

					await context.SaveChangesAsync();
					#endregion
				}


				response.ResponseID = 1;
				response.ResponseValue = true;

			}
			return response;
		}


		#region Configure Json File to Check Monthly Salary 
		private ConfigModel LoadConfig()
		{
			if (File.Exists(ConfigFilePath))
			{
				var json = File.ReadAllText(ConfigFilePath);
				return JsonConvert.DeserializeObject<ConfigModel>(json);
			}
			else
			{
				return new ConfigModel();
			}
		}
		private void SaveConfig(ConfigModel config)
		{
			var json = JsonConvert.SerializeObject(config, Formatting.Indented);
			File.WriteAllText(ConfigFilePath, json);
		}
		#endregion
	}
}
