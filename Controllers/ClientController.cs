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

		[HttpPost]
		public async Task<IActionResult> AddClientAsync(AddClientDto clientDto)
		{
			if (clientDto == null)
				return BadRequest("Please Enter Farm Name");
			var client = await clientRepo.AddClientAsync(clientDto);
			if (client.ClientID != 0)
			{
				return Ok(client);
			}
			return Conflict("There is farm exists with the same name");
		}

		[HttpGet]
		public async Task<IActionResult> GetAllClients()
		{
			var allClients = await clientRepo.GetClientAsync();
			if (allClients.Count() != 0)
				return Ok(allClients);
			return Conflict("No Data Found");
		}

		[HttpGet("id:int")]
		public async Task<IActionResult> GetClientById(int id)
		{
			var clientdb = await clientRepo.GetClientById(id);
			if (clientdb.ClientID == 0)
				return NotFound($"No Farm Found By ID {id}");
			return Ok(clientdb);
		}

		[HttpPut("id:int")]
		public async Task<IActionResult> UpdateClient(int id, [FromBody] AddClientDto clientDto)
		{
			var clientDb = await clientRepo.GetClientById(id);
			if (clientDb.ClientID == 0)
				return NotFound($"No farm found by this {id}");
			if (clientDto.ClientName == "")
				return BadRequest("Please Enter Farm Name");

			var clientUpdated = await clientRepo.UpdateClient(id, clientDto);
			return Ok(clientUpdated);
		}
	}
}
