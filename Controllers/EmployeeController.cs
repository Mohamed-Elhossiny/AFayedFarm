using AFayedFarm.Dtos;
using AFayedFarm.Repositories.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeRepo repo;

		public EmployeeController(IEmployeeRepo repo)
		{
			this.repo = repo;
		}
		[HttpPost("~/AddEmployee")]
		public async Task<IActionResult> AddEmployee(AddEmployeeDto dto)
		{
			if (dto.Name == "")
				return BadRequest("Enter Employee Name and Salary");
			var response = await repo.AddEmployee(dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound(dto);
		}

		[HttpGet("~/GetEmployee")]
		public async Task<IActionResult> GetEmployee(int id)
		{
			if (id == 0)
				return BadRequest("Invalid ID");
			var response = await repo.GetEmployee(id);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound($"There is no Employee with this id {id}");

		}

		[HttpPut("~/UpdateEmployee")]
		public async Task<IActionResult> UpdateEmployee(int id, AddEmployeeDto dto)
		{
			if (id == 0)
				return BadRequest("Invalid ID");
			if(dto.Name =="" || dto.Salary == 0)
				return BadRequest("Enter Employee Name and Salary");
			var response = await repo.UpdateEmployee(id, dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}

		[HttpGet("~/GetAllEmployees")]
		public async Task<IActionResult> GetAllEmployees(int currentPage= 1, int pageSize=100)
		{
			var response = await repo.GetAllEmployee(currentPage,pageSize);
			if (response.ResponseID == 1)
				//return Ok(response);
			return Ok(response.ResponseValue);
			else
				response.ResponseMessage = "There is no employee";
				//return Ok(response);
				return NotFound();
		}

		[HttpPost("~/PayToEmployee")]
		public async Task<IActionResult> PayToEmployee(EmployeePaymentDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await repo.PayToEmployee(dto);
			if (response.ResponseID == 0)
				return NotFound();
			else
				return Ok(response.ResponseValue);
		}
	}
}
