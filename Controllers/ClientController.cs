using AFayedFarm.Dtos;
using AFayedFarm.Repositories.Clients;
using AFayedFarm.Repositories.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFayedFarm.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientController : ControllerBase
	{
		private readonly IClientRepo clientRepo;

		public ClientController(IClientRepo clientRepo)
		{
			this.clientRepo = clientRepo;
		}

		[HttpPost("~/AddClient")]
		public async Task<IActionResult> AddClientAsync(AddClientDto clientDto)
		{
			if (clientDto == null)
				return BadRequest("Please Enter Farm Name");
			var client = await clientRepo.AddClientAsync(clientDto);
			if (client.ID != 0)
			{
				return Ok(client);
			}
			return Conflict("There is client exists with the same name");
		}

		[HttpGet("~/GetAllClients")]
		public async Task<IActionResult> GetAllClients()
		{
			var allClients = await clientRepo.GetClientAsync();
			if (allClients.Count() != 0)
				return Ok(allClients);
			return Conflict("No Data Found");
		}

		[HttpGet("~/GetClientById")]
		public async Task<IActionResult> GetClientById(int id)
		{
			var clientdb = await clientRepo.GetClientById(id);
			if (clientdb.ResponseID == 0)
				return NotFound($"No Farm Found By ID {id}");
			return Ok(clientdb.ResponseValue);
		}

		[HttpPut("~/UpdateClient")]
		public async Task<IActionResult> UpdateClient(int id, [FromBody] AddClientDto clientDto)
		{
			var clientDb = await clientRepo.GetClientById(id);
			if (clientDb.ResponseID == 0)
				return NotFound($"No farm found by this {id}");
			if (clientDto.Name == "")
				return BadRequest("Please Enter Farm Name");

			var clientUpdated = await clientRepo.UpdateClient(id, clientDto);
			if (clientUpdated.ResponseID == 1)
				return Ok(clientUpdated);
			else return BadRequest(clientUpdated.ResponseValue);
		}

		//[HttpPost("~/AddTransaction")]
		//public async Task<IActionResult> AddTransaction(AddTransactionDto dto)
		//{
		//	if (dto.ProductID == 0 || dto.ProductID == null)
		//		return BadRequest("You must enter product");
		//	if (dto.ClientID == 0 || dto.ClientID == null)
		//		return BadRequest("You must enter clientID");

		//	var response = await clientRepo.AddTransaction(dto);
		//	if (response.ResponseID == 2)
		//		return NotFound($"There is no quantity for this product {dto.ProductID}");
		//	if (response.ResponseID == 3)
		//		return NotFound($"There is no enough quantit for this product {dto.ProductID} in our store");
		//	if (response.ResponseID == 0)
		//		return NotFound();
		//	else
		//		return Ok(response.ResponseValue);
		//}

		[HttpGet("~/GetTransactionByCleintID")]
		public async Task<IActionResult> GetTransactionByCleintID(int clientID)
		{
			if (clientID == 0)
				return BadRequest();
			var response = await clientRepo.GetTransactionsByClientId(clientID);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound("There is no transactions for that client");
		}
	}
}
