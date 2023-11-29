using AFayedFarm.Dtos;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Repositories.Clients
{
	public class ClientRepo : IClientRepo
	{
		private readonly FarmContext context;

		public ClientRepo(FarmContext context)
		{
			this.context = context;
		}
		public async Task<ClientDto> AddClientAsync(AddClientDto clientDto)
		{
			var client = new ClientDto();
			var clientdb = context.Clients.Where(f => f.ClientName.ToLower() == clientDto.ClientName.ToLower()).FirstOrDefault();
			if (clientdb == null)
			{
				var Client = new Client()
				{
					ClientName = clientDto.ClientName,
				};
				await context.Clients.AddAsync(Client);
				await context.SaveChangesAsync();
				client.ClientName = Client.ClientName;
				client.ClientID = Client.ClientID;
				return client;
			}

			client.ClientID = 0;
			return client;
		}

		public async Task<List<ClientDto>> GetClientAsync()
		{
			var clientsDb = await context.Clients.Select(c => new ClientDto
			{
				ClientID = c.ClientID,
				ClientName = c.ClientName
			}).ToListAsync();
			return clientsDb;
		}

		public async Task<ClientDto> GetClientById(int id)
		{
			var clientDb = await context.Clients.FindAsync(id);
			if (clientDb != null)
			{
				var Client = new ClientDto()
				{
					ClientName = clientDb.ClientName,
					ClientID = clientDb.ClientID,
				};
				return Client;
			}
			return new ClientDto { ClientID = 0 };
		}

		public async Task<ClientDto> GetClientByName(string clientName)
		{
			var client = new ClientDto();
			var clientdb = await context.Clients.Where(c => c.ClientName.ToLower() == clientName.ToLower()).FirstOrDefaultAsync();
			if (clientdb != null)
			{
				var Client = new ClientDto()
				{
					ClientName = clientdb.ClientName,
					ClientID = clientdb.ClientID,
				};
				return Client;
			}
			client.ClientID = 0;
			return client;
		}

		public async Task<ClientDto> UpdateClient(int id, AddClientDto clientDto)
		{
			var clientDb = await context.Clients.SingleOrDefaultAsync(c => c.ClientID == id);
			clientDb.ClientName = clientDto.ClientName;
			await context.SaveChangesAsync();

			return new ClientDto { ClientID = clientDb.ClientID, ClientName = clientDb.ClientName };
		}
	}
}
