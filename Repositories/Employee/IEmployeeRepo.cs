using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Employee
{
	public interface IEmployeeRepo
	{
		Task<RequestResponse<EmployeeDto>> AddEmployee(AddEmployeeDto dto);
		Task<RequestResponse<EmployeeDto>> GetEmployee(int empId);
		Task<RequestResponse<EmployeeDto>> UpdateEmployee(int empId,AddEmployeeDto dto);
		Task<RequestResponse<List<EmployeeDto>>> GetAllEmployee();
		Task<RequestResponse<bool>> PayToEmployee(EmployeePaymentDto dto);
		Task<RequestResponse<bool>> PayMonlthySalary();

	}
}
