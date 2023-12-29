using AFayedFarm.Dtos;
using AFayedFarm.Enums;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Repositories.Employee
{
	public class EmployeeRepo : IEmployeeRepo
	{
		private readonly FarmContext context;

		public EmployeeRepo(FarmContext context)
		{
			this.context = context;
		}
		public async Task<RequestResponse<EmployeeDto>> AddEmployee(AddEmployeeDto dto)
		{
			var response = new RequestResponse<EmployeeDto> { ResponseID = 0 };
			var empolyee = new EmployeeDto();
			var empdb = context.Employees.Where(f => f.Full_Name.ToLower() == dto.Name.ToLower()).FirstOrDefault();
			if (empdb == null)
			{
				var emp = new Model.Employee()
				{
					Full_Name = dto.Name,
					Salary = dto.Salary,
				};
				await context.Employees.AddAsync(emp);
				await context.SaveChangesAsync();
				empolyee.Name = emp.Full_Name;
				empolyee.ID = emp.EmpolyeeID;
				empolyee.Salary = emp.Salary;
				response.ResponseID = 1;
				response.ResponseValue = empolyee;
				return response;
			}
			return response;
		}

		public async Task<RequestResponse<List<EmployeeDto>>> GetAllEmployee()
		{
			var response = new RequestResponse<List<EmployeeDto>> { ResponseID = 0, ResponseValue = new List<EmployeeDto>() };
			var empsDb = await context.Employees.Select(f => new EmployeeDto
			{
				ID = f.EmpolyeeID,
				Name = f.Full_Name,
				Salary = f.Salary
			}).ToListAsync();

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

			}

			return response;

		}

		public async Task<RequestResponse<bool>> PayToEmployee(EmployeePaymentDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			var empDb = await context.Employees.Where(e => e.EmpolyeeID == dto.Id).FirstOrDefaultAsync();
			if (empDb == null)
				return response;
			var financialSafe = await context.Safe.FindAsync(1);
			var transaction = new SafeTransaction()
			{
				Emp_ID = dto.Id,
				TypeID = dto.TrasactionTypeID,
				Total = -1 * dto.Total,
				Type = ((TransactionType)dto.TrasactionTypeID!).ToString(),
				Notes = dto.Notes
			};
			if (dto.TrasactionTypeID == 2)
				financialSafe!.Total = financialSafe.Total - dto.Total;
			await context.SafeTransactions.AddAsync(transaction);
			context.Safe.Update(financialSafe!);
			await context.SaveChangesAsync();
			response.ResponseID = 1;
			response.ResponseValue = true;
			return response;
		}
	}
}
