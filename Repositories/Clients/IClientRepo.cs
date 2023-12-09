using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Clients
{
	public interface IClientRepo
	{
		//////////// TO DO ///////////
		//Add RequestResponse as a return for old Function
		Task<ClientDto> AddClientAsync(AddClientDto clientDto);
		Task<ClientDto> GetClientByName(string clientName);
		Task<List<ClientDto>> GetClientAsync();
		Task<ClientDto> GetClientById(int id);
		Task<ClientDto> UpdateClient(int id, AddClientDto clientDto);
		Task<RequestResponse<TransactionDto>> AddTransaction(AddTransactionDto dto);
		Task<RequestResponse<TransactionDto>> GetTransactionByRecordId(int transaxtionid);
		Task<RequestResponse<List<TransactionDto>>> GetTransactionsByClientId(int clientid);
		
	}
}
