using AFayedFarm.Dtos;
using AFayedFarm.Global;
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

		public async Task<RequestResponse<TransactionDto>> GetTransactionByRecordId(int recordid)
		{
			var response = new RequestResponse<TransactionDto> { ResponseID = 0, ResponseValue = new TransactionDto() };
			var transaction = await context.Transactions.Where(c => c.TransactionID == recordid).Include(c => c.Product).Include(c => c.Client).FirstOrDefaultAsync();
			if (transaction != null)
			{
				var transactionDto = new TransactionDto();

				transactionDto.TransactionID = transaction.TransactionID;
				transactionDto.StoreID = transaction.StoreID;
				transactionDto.ClientID = transaction.ClientID;
				transactionDto.ClientName = transaction?.Client?.ClientName ?? "";
				transactionDto.ProductID = transaction?.ProductID;
				transactionDto.ProductName = transaction?.Product?.ProductName ?? "";
				transactionDto.ShippingDate = transaction?.ShippingDate;
				transactionDto.Created_Date = transaction?.Created_Date ?? DateTime.Now;
				transactionDto.Quantity = transaction?.Quantity;
				transactionDto.Total = transaction?.Total;
				transactionDto.GetPaied = transaction?.GetPaied;
				transactionDto.Remaining = transaction?.Remaining;
				transactionDto.Notes = transaction?.Notes;

				response.ResponseID = 1;
				response.ResponseValue = transactionDto;
			}
			return response;

		}

		public async Task<RequestResponse<TransactionDto>> AddTransaction(AddTransactionDto dto)
		{
			var response = new RequestResponse<TransactionDto> { ResponseID = 0, ResponseValue = new TransactionDto() };
			var productList = await context.StoreProducts.OrderBy(c => c.ProductID).ToListAsync();
			var product = productList.Where(c => c.ProductID == dto.ProductID).FirstOrDefault();
			var quantity = product?.Quantity;
			if (quantity == 0)
			{
				response.ResponseID = 2;
				return response;
			}
			if (quantity > 0 && quantity < dto.Quantity)
			{
				response.ResponseID = 3;
				return response;
			}
			decimal? remaining = 0;
			remaining = dto.Total - dto.GetPaied;
			var transaction = new Transaction();

			transaction.StoreID = 1;
			transaction.ClientID = (int)dto.ClientID;
			transaction.ProductID = (int)dto.ProductID;
			transaction.ShippingDate = dto.ShippingDate ?? DateTime.Now;
			transaction.Created_Date = dto.Created_Date ?? DateTime.Now;
			transaction.Quantity = dto.Quantity;
			transaction.Total = dto.Total;
			transaction.GetPaied = dto.GetPaied;
			transaction.Remaining = remaining;
			transaction.Notes = dto.Notes;

			await context.Transactions.AddAsync(transaction);
			await context.SaveChangesAsync();

			await UpdateProductQuantityInStore(dto);

			// TO DO ==> Get Transaction Record

			var transactionDto = await GetTransactionByRecordId(transaction.TransactionID);
			if (transactionDto.ResponseID == 1)
			{
				response.ResponseID = 1;
				response.ResponseValue = transactionDto.ResponseValue;
				return response;
			}

			response.ResponseID = 1;
			return response;
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

		public async Task<RequestResponse<ClientDto>> GetClientById(int id)
		{
			var response = new RequestResponse<ClientDto> { ResponseID = 0, ResponseValue = new ClientDto() };
			var clientDb = await context.Clients.FindAsync(id);
			if (clientDb != null)
			{
				var Client = new ClientDto()
				{
					Name = clientDb.ClientName,
					ID = clientDb.ClientID,

					//############ TO DO Check Total with Hossam
					Total = 0
				};
				response.ResponseID = 1;
				response.ResponseValue = Client;
			}
			return response;

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

		public async Task<RequestResponse<ClientDto>> UpdateClient(int id, AddClientDto clientDto)
		{
			var response = new RequestResponse<ClientDto> { ResponseID = 0, ResponseValue = new ClientDto() };
			var clientDb = await context.Clients.SingleOrDefaultAsync(c => c.ClientID == id);
			if (clientDb != null)
			{
				clientDb.ClientName = clientDto.Name;
				await context.SaveChangesAsync();

				var client = await GetClientById(id);
				response.ResponseID = 1;
				response.ResponseValue = client.ResponseValue;
			}
			return response;
		}

		public async Task<RequestResponse<bool>> UpdateProductQuantityInStore(AddTransactionDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			var productInStore = await context.StoreProducts.Where(c => c.ProductID == dto.ProductID).FirstOrDefaultAsync();
			if (productInStore != null)
			{
				productInStore.Quantity -= dto.Quantity;
				context.StoreProducts.Update(productInStore);
				await context.SaveChangesAsync();
				response.ResponseID = 1;
				response.ResponseValue = true;
			}
			return response;

		}

		public async Task<RequestResponse<List<TransactionDto>>> GetTransactionsByClientId(int clientId)
		{
			var response = new RequestResponse<List<TransactionDto>> { ResponseID = 0, ResponseValue = new List<TransactionDto>() };
			var transactions = await context.Transactions
				.Include(c => c.Product)
				.Include(c => c.Client)
				.Where(c => c.ClientID == clientId)
				.ToListAsync();
			if (transactions.Count != 0)
			{
				var transactionListDto = new List<TransactionDto>();

				foreach (var item in transactions)
				{
					var transactionDto = new TransactionDto();
					transactionDto.TransactionID = item.TransactionID;
					transactionDto.StoreID = item.StoreID;
					transactionDto.ClientID = item.ClientID;
					transactionDto.ClientName = item?.Client?.ClientName ?? "";
					transactionDto.ProductID = item?.ProductID;
					transactionDto.ProductName = item?.Product?.ProductName ?? "";
					transactionDto.ShippingDate = item?.ShippingDate;
					transactionDto.Created_Date = item?.Created_Date ?? DateTime.Now;
					transactionDto.Quantity = item?.Quantity;
					transactionDto.Total = item?.Total;
					transactionDto.GetPaied = item?.GetPaied;
					transactionDto.Remaining = item?.Remaining;
					transactionDto.Notes = item?.Notes;

					transactionListDto.Add(transactionDto);
				}
				response.ResponseID = 1;
				response.ResponseValue = transactionListDto;
			}
			return response;
		}

	}
}
