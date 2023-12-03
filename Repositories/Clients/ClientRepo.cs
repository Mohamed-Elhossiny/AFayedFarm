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
			var clientdb = context.Clients.Where(f => f.ClientName.ToLower() == clientDto.Name.ToLower()).FirstOrDefault();
			if (clientdb == null)
			{
				var Client = new Client()
				{
					ClientName = clientDto.Name,
				};
				await context.Clients.AddAsync(Client);
				await context.SaveChangesAsync();
				client.Name = Client.ClientName;
				client.ID = Client.ClientID;
				return client;
			}

			client.ID = 0;
			return client;
		}

		public async Task<List<ClientDto>> GetClientAsync()
		{
			var clientsDb = await context.Clients.Select(c => new ClientDto
			{
				ID = c.ClientID,
				Name = c.ClientName
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
					Name = clientDb.ClientName,
					ID = clientDb.ClientID,
				};
				return Client;
			}
			return new ClientDto { ID = 0 };
		}

		public async Task<ClientDto> GetClientByName(string clientName)
		{
			var client = new ClientDto();
			var clientdb = await context.Clients.Where(c => c.ClientName.ToLower() == clientName.ToLower()).FirstOrDefaultAsync();
			if (clientdb != null)
			{
				var Client = new ClientDto()
				{
					Name = clientdb.ClientName,
					ID = clientdb.ClientID,
				};
				return Client;
			}
			client.ID = 0;
			return client;
		}

		public async Task<ClientDto> UpdateClient(int id, AddClientDto clientDto)
		{
			var clientDb = await context.Clients.SingleOrDefaultAsync(c => c.ClientID == id);
			clientDb.ClientName = clientDto.Name;
			await context.SaveChangesAsync();

			return new ClientDto { ID = clientDb.ClientID, Name = clientDb.ClientName };
		}
	}
}
