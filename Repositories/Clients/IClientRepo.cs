using AFayedFarm.Dtos;
using AFayedFarm.Dtos.Client;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Clients
{
	public interface IClientRepo
	{
		////////////// TO DO ///////////
		////Add RequestResponse as a return for old Function
		Task<ClientDto> AddClientAsync(AddClientDto clientDto);
		Task<ClientDto> GetClientByName(string clientName);
		Task<List<ClientDto>> GetClientAsync();
		Task<RequestResponse<ClientDto>> GetClientById(int id);
		Task<RequestResponse<ClientDto>> UpdateClient(int id, AddClientDto clientDto);
		Task<RequestResponse<TransactionProductDto>> AddTransaction(AddTransactionMainDataDto dto);
		Task<RequestResponse<TransactionDto>> GetTransactionByRecordId(int transaxtionid);
		Task<RequestResponse<List<TransactionDto>>> GetTransactionsByClientId(int clientid);

	}
}
