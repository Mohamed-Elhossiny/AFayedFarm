using AFayedFarm.Dtos;

namespace AFayedFarm.Repositories.Clients
{
	public interface IClientRepo
	{
		Task<ClientDto> AddClientAsync(AddClientDto clientDto);
		Task<ClientDto> GetClientByName(string clientName);
		Task<List<ClientDto>> GetClientAsync();
		Task<ClientDto> GetClientById(int id);
		Task<ClientDto> UpdateClient(int id, AddClientDto clientDto);
	}
}
