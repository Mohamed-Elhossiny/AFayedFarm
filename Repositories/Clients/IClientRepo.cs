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
		Task<RequestResponse<TransactionMainDataDto>> AddTransaction(AddTransactionMainDataDto dto);
		Task<RequestResponse<TransactionMainDataDto>> GetTransactionByRecordId(int transactionid);
		Task<RequestResponse<List<TransactionMainDataDto>>> GetTransactionsByClientId(int clientid);
		Task<RequestResponse<TransactionWithClientData>> GetTransactionsWithCleintData(int clientid, int currentPage, int pageSize);
		Task<RequestResponse<ClientDto>> CollectMoneyFromClient(CollectMoneyDto dto);
		Task<RequestResponse<TransactionMainDataDto>> UpdateClientRecord(int id, AddTransactionMainDataDto dto);
		Task<RequestResponse<bool>> ReturnProductToStore(AddProductListDto dto);
		Task<RequestResponse<bool>> ReturnProductBoxToStore(AddProductListDto dto);

	}
}
