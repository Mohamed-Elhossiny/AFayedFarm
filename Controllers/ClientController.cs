using AFayedFarm.Dtos;
using AFayedFarm.Dtos.Client;
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

		[HttpPost("~/AddTransaction")]
		public async Task<IActionResult> AddTransaction([FromBody]AddTransactionMainDataDto dto)
		{

			if (dto.ClientID == 0 || dto.ClientID == null)
				return BadRequest("You must enter clientID");

			var response = await clientRepo.AddTransaction(dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound();
		}

		[HttpGet("~/GetTransactionsWithClientData")]
		public async Task<IActionResult> GetTransactionsWithClientData(int clientID)
		{
			if (clientID == 0)
				return BadRequest();
			var response = await clientRepo.GetTransactionsWithCleintData(clientID);
			return Ok(response.ResponseValue);

		}

		[HttpGet("~/GetTransactionRecordByID")]
		public async Task<IActionResult> GetTransactionRecordByID(int recordID)
		{
			if (recordID == 0)
				return BadRequest();
			var response = await clientRepo.GetTransactionByRecordId(recordID);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return NotFound($"There is no transactions for that ID {recordID}");
		}

		[HttpPost("~/CollectMoneyFromClient")]
		public async Task<IActionResult> CollectMoneyFromClient(CollectMoneyDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await clientRepo.CollectMoneyFromClient(dto);
			if (response.ResponseID == 0)
				return NotFound();
			else
				return Ok(response.ResponseValue);
		}

		[HttpPut("~/UpdateClientRecord")]
		public async Task<IActionResult> UpdateClientRecord(int id, [FromBody] AddTransactionMainDataDto dto)
		{
			if (id == 0)
				return BadRequest("Enter Valid Id");
			var response = await clientRepo.UpdateClientRecord(id,dto);
			if (response.ResponseID == 1)
				return Ok(response.ResponseValue);
			else
				return BadRequest(dto);

		}
	}
}
