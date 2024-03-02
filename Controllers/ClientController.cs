using AFayedFarm.Dtos;
using AFayedFarm.Dtos.Client;
using AFayedFarm.Global;
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
			var response = new RequestResponse<ClientDto>() { ResponseID = 0, ResponseValue = new ClientDto() };
			if (client.ID != 0)
			{
				response.ResponseID = 1;
				response.ResponseValue = client;
				return Ok(response);
			}
			return Ok(response);
			//return Conflict("There is client exists with the same name");
		}

		[HttpGet("~/GetAllClients")]
		public async Task<IActionResult> GetAllClients()
		{
			var response = new RequestResponse<List<ClientDto>> { ResponseID = 0, ResponseValue = new List<ClientDto>() };
			var allClients = await clientRepo.GetClientAsync();
			if (allClients.Count() != 0)
			{
				response.ResponseID = 1;
				response.ResponseValue = allClients;
				return Ok(response);
			}
			return Ok(response);
		}

		[HttpGet("~/GetClientById")]
		public async Task<IActionResult> GetClientById(int id)
		{
			var clientdb = await clientRepo.GetClientById(id);
			if (clientdb.ResponseID == 0)
				return Ok(clientdb);
			//return NotFound($"No Farm Found By ID {id}");
			//return Ok(clientdb.ResponseValue);
			return Ok(clientdb);
		}

		[HttpPut("~/UpdateClient")]
		public async Task<IActionResult> UpdateClient(int id, [FromBody] AddClientDto clientDto)
		{
			var clientDb = await clientRepo.GetClientById(id);
			if (clientDb.ResponseID == 0)
			{
				clientDb.ResponseMessage = $"No farm found by this {id}";
				return Ok(clientDb);
			}

			//return NotFound($"No farm found by this {id}");
			if (clientDto.Name == "")
			{
				clientDb.ResponseMessage = "Please Enter Farm Name";
				clientDb.ResponseID = 0;
				return Ok(clientDb);
			}
			var clientUpdated = await clientRepo.UpdateClient(id, clientDto);
			if (clientUpdated.ResponseID == 1)
				return Ok(clientUpdated);
			else
			{
				clientUpdated.ResponseID = 0;
				return Ok(clientUpdated);
			}
		}

		[HttpPost("~/AddTransaction")]
		public async Task<IActionResult> AddTransaction([FromBody] AddTransactionMainDataDto dto)
		{

			if (dto.ClientID == 0 || dto.ClientID == null)
				return BadRequest("You must enter clientID");

			var response = await clientRepo.AddTransaction(dto);
			if (response.ResponseID == 1)
				return Ok(response);
			else
				return Ok(response);
		}

		[HttpGet("~/GetTransactionsWithClientData")]
		public async Task<IActionResult> GetTransactionsWithClientData(int clientID, int currentPage = 1, int pageSize = 500)
		{
			if (clientID == 0)
				return BadRequest();
			var response = await clientRepo.GetTransactionsWithCleintData(clientID, currentPage, pageSize);
			return Ok(response);
		}

		[HttpGet("~/GetTransactionRecordByID")]
		public async Task<IActionResult> GetTransactionRecordByID(int recordID)
		{
			if (recordID == 0)
				return BadRequest();
			var response = await clientRepo.GetTransactionByRecordId(recordID);
			if (response.ResponseID == 1)
				return Ok(response);
			else
			{
				response.ResponseMessage = $"There is no transactions for that ID {recordID}";
				response.ResponseID = 0;

				return Ok(response);
				//return NotFound($"There is no transactions for that ID {recordID}");
			}
		}

		[HttpPost("~/CollectMoneyFromClient")]
		public async Task<IActionResult> CollectMoneyFromClient(CollectMoneyDto dto)
		{
			if (dto.Id == 0)
				return BadRequest("Enter Valid Id");
			var response = await clientRepo.CollectMoneyFromClient(dto);
			if (response.ResponseID == 0)
				return Ok(response);
			else
				return Ok(response);
		}

		[HttpPut("~/UpdateClientRecord")]
		public async Task<IActionResult> UpdateClientRecord(int id, [FromBody] AddTransactionMainDataDto dto)
		{
			if (id == 0)
				return BadRequest("Enter Valid Id");
			var response = await clientRepo.UpdateClientRecord(id, dto);
			if (response.ResponseID == 1)
				return Ok(response);
			else
				return Ok(response);

		}

		[HttpDelete("~/Delete Product")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			if (id == 0)
				return BadRequest($"Enter valid id {id}");
			var resonse = await clientRepo.DeleteProductItem(id);
			return Ok(resonse);
		}
	}
}
