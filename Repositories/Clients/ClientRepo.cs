using AFayedFarm.Dtos;
using AFayedFarm.Dtos.Client;
using AFayedFarm.Enums;
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
			var clientdb = context.Clients.Where(f => f.ClientName!.ToLower() == clientDto.Name.ToLower()).FirstOrDefault();
			if (clientdb == null)
			{
				var Client = new Client()
				{
					ClientName = clientDto.Name,
					Create_Date = DateTime.Now.Date
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

		public async Task<RequestResponse<TransactionMainDataDto>> GetTransactionByRecordId(int recordid)
		{
			var response = new RequestResponse<TransactionMainDataDto> { ResponseID = 0, ResponseValue = new TransactionMainDataDto() };
			var transaction = await context.Transactions.Where(c => c.TransactionID == recordid).Include(c => c.Client).Include(c => c.TransactionProducts!).ThenInclude(c => c.Product).FirstOrDefaultAsync();
			if (transaction != null)
			{
				var transactionDto = new TransactionMainDataDto();

				var productListDto = new List<ProductListDto>();

				transactionDto.ID = transaction.TransactionID;
				transactionDto.ClientID = transaction.ClientID;
				transactionDto.ClientName = transaction?.Client?.ClientName ?? "";
				transactionDto.DriverName = transaction?.DriverName ?? "";
				transactionDto.Date = transaction?.Created_Date ?? DateTime.Now.Date;
				transactionDto.Notes = transaction?.Notes ?? "";
				transactionDto.Price = transaction?.Price ?? 0;
				transactionDto.Total = transaction?.Total ?? 0;
				transactionDto.Payed = transaction?.Payed ?? 0;
				transactionDto.CarCapacity = transaction?.TotalCapcity ?? 0;
				transactionDto.Remaining = transaction?.Remaining ?? 0;
				transactionDto.TypeId = (int)TransactionType.Income;

				foreach (var item in transaction?.TransactionProducts)
				{
					var productDto = new ProductListDto();
					if (item.Qunatity != null)
					{
						productDto.ProductID = item.Product?.ProductID;
						productDto.ProductName = item.Product?.ProductName;
						productDto.Quantity = item.Qunatity;
					}
					if (item.Number != null)
					{
						productDto.ProductBoxName = item.Product?.ProductName;
						productDto.ProductBoxID = item.Product?.ProductID;
						productDto.Number = item.Number;
					}
					productListDto.Add(productDto);
				}
				transactionDto.ProductList = productListDto;
				response.ResponseID = 1;
				response.ResponseValue = transactionDto;
			}
			return response;


		}

		public async Task<RequestResponse<TransactionMainDataDto>> AddTransaction(AddTransactionMainDataDto dto)
		{
			var response = new RequestResponse<TransactionMainDataDto> { ResponseID = 0, ResponseValue = new TransactionMainDataDto() };
			var transaction = new Transaction();
			transaction.ClientID = (int)dto.ClientID!;
			transaction.ShippingDate = (DateTime)dto.Date!;
			transaction.Created_Date = DateTime.Now.Date;
			transaction.Price = dto.Price;
			transaction.Total = dto.Total;
			transaction.Payed = dto.Payed;
			transaction.Remaining = (dto.Total - dto.Payed);
			transaction.DriverName = dto.DriverName;
			transaction.TotalCapcity = dto.CarCapacity;

			await context.Transactions.AddAsync(transaction);
			await context.SaveChangesAsync();

			/// Asscoiate Product List to This Transaction

			if (dto.ProductList!.Count != 0)
			{
				foreach (var item in dto.ProductList)
				{
					if (item.ProductID != 0)
					{
						var transactionProduct = new TransactionProduct();
						transactionProduct.TransactionID = transaction.TransactionID;
						transactionProduct.ProductID = item.ProductID;
						transactionProduct.Qunatity = item.Quantity;

						await context.TransactionProducts.AddAsync(transactionProduct);
						await context.SaveChangesAsync();


					}
					if (item.ProductBoxID != 0)
					{
						var transactionProduct = new TransactionProduct();
						transactionProduct.TransactionID = transaction.TransactionID;
						transactionProduct.ProductID = item.ProductBoxID;
						transactionProduct.Number = item.Number;

						await context.TransactionProducts.AddAsync(transactionProduct);
						await context.SaveChangesAsync();

					}
				}

				await UpdateProductQuantityInStore(dto);
			}

			#region Add Transactions to Financial Safe

			var safeTransaction = new SafeTransaction();
			safeTransaction.SafeID = 1;
			safeTransaction.CLientID = dto.ClientID;
			safeTransaction.TypeID = dto.TypeId;
			safeTransaction.Type = ((TransactionType)dto.TypeId!).ToString();
			safeTransaction.Total = dto.Payed;
			safeTransaction.Notes = dto.Notes;

			await context.SafeTransactions.AddAsync(safeTransaction);

			var financialSafe = await context.Safe.FindAsync(1);
			if (dto.TypeId == (int)TransactionType.Income)
				financialSafe!.Total = financialSafe.Total + dto.Payed;

			context.Safe.Update(financialSafe!);

#warning Check Total for the client with Hossam

			var clientDb = await context.Clients.Where(f => f.ClientID == dto.ClientID).FirstOrDefaultAsync();
			if (clientDb.Total == null)
				clientDb.Total = 0;
			clientDb!.Total += -1 * (dto.Total - dto.Payed);
			context.Clients.Update(clientDb);

			await context.SaveChangesAsync();
			#endregion

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
				Name = c.ClientName,
				Total = c.Total,
				Created_Date = DateOnly.FromDateTime(c.Create_Date ?? DateTime.Now)
			}).OrderByDescending(c => c.ID).ToListAsync();
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
					Total = clientDb.Total,
					Created_Date = DateOnly.FromDateTime(clientDb.Create_Date ?? DateTime.Now)
				};
				response.ResponseID = 1;
				response.ResponseValue = Client;
			}
			return response;

		}

		public async Task<ClientDto> GetClientByName(string clientName)
		{
			var client = new ClientDto();
			var clientdb = await context.Clients.Where(c => c.ClientName!.ToLower() == clientName.ToLower()).FirstOrDefaultAsync();
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

		public async Task<RequestResponse<bool>> UpdateProductQuantityInStore(AddTransactionMainDataDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			foreach (var item in dto.ProductList!)
			{
				if (item.ProductID != null)
				{
					var productInStore = await context.StoreProducts.Where(c => c.ProductID == item.ProductID).FirstOrDefaultAsync();
					if (productInStore != null)
					{
						if (productInStore.Quantity == null)
							productInStore.Quantity = 0;
						productInStore.Quantity -= item.Quantity;
						context.StoreProducts.Update(productInStore);
						await context.SaveChangesAsync();
					}
				}
				if (item.ProductBoxID != null)
				{
					var productInStore = await context.StoreProducts.Where(c => c.ProductID == item.ProductBoxID).FirstOrDefaultAsync();
					if (productInStore != null)
					{
						if (productInStore.Quantity == null)
							productInStore.Quantity = 0;
						productInStore.Quantity -= item.Number;
						context.StoreProducts.Update(productInStore);
						await context.SaveChangesAsync();
					}
				}
			}
			response.ResponseID = 1;
			response.ResponseValue = true;
			return response;
		}

		public async Task<RequestResponse<List<TransactionMainDataDto>>> GetTransactionsByClientId(int clientId)
		{
			var response = new RequestResponse<List<TransactionMainDataDto>> { ResponseID = 0, ResponseValue = new List<TransactionMainDataDto>() };
			var transactions = await context.Transactions.Where(c => c.ClientID == clientId).Include(c => c.Client).Include(c => c.TransactionProducts!).ThenInclude(c => c.Product).ToListAsync();
			if (transactions.Count != 0)
			{
				var transactionListDto = new List<TransactionMainDataDto>();

				foreach (var transaction in transactions)
				{
					var transactionDto = new TransactionMainDataDto();
					var productListDto = new List<ProductListDto>();


					transactionDto.ID = transaction.TransactionID;
					transactionDto.ClientID = transaction.ClientID;
					transactionDto.ClientName = transaction?.Client?.ClientName ?? "";
					transactionDto.DriverName = transaction?.DriverName ?? "";
					transactionDto.Date = transaction?.Created_Date ?? DateTime.Now.Date;
					transactionDto.Notes = transaction?.Notes ?? "";
					transactionDto.Price = transaction?.Price ?? 0;
					transactionDto.Total = transaction?.Total ?? 0;
					transactionDto.Payed = transaction?.Payed ?? 0;
					transactionDto.CarCapacity = transaction?.TotalCapcity ?? 0;
					transactionDto.Remaining = transaction?.Remaining ?? 0;
					transactionDto.TypeId = (int)TransactionType.Income;

					foreach (var item in transaction?.TransactionProducts)
					{
						var productDto = new ProductListDto();
						if (item.Qunatity != null)
						{
							productDto.ProductID = item.Product?.ProductID;
							productDto.ProductName = item.Product?.ProductName;
							productDto.Quantity = item.Qunatity;
						}
						if (item.Number != null)
						{
							productDto.ProductBoxName = item.Product?.ProductName;
							productDto.ProductBoxID = item.Product?.ProductID;
							productDto.Number = item.Number;
						}
						productListDto.Add(productDto);
					}
					transactionDto.ProductList = productListDto;

					transactionListDto.Add(transactionDto);
				}
				response.ResponseID = 1;
				response.ResponseValue = transactionListDto;
			}
			return response;
		}

		public async Task<RequestResponse<ClientDto>> CollectMoneyFromClient(CollectMoneyDto dto)
		{
			var response = new RequestResponse<ClientDto> { ResponseID = 0, ResponseValue = new ClientDto() };
			var clientDb = await context.Clients.Where(e => e.ClientID == dto.Id).FirstOrDefaultAsync();
			if (clientDb == null)
				return response;

			#region Add Transactions to Financial Safe
			var transaction = new SafeTransaction()
			{
				SafeID = 1,
				CLientID = dto.Id,
				TypeID = dto.TrasactionTypeID,
				Type = ((TransactionType)dto.TrasactionTypeID!).ToString(),
				Total = dto.Total,
				Notes = dto.Notes,
				Created_Date = DateTime.Now.Date
			};
			await context.SafeTransactions.AddAsync(transaction);

			var financialSafe = await context.Safe.FindAsync(1);
			if (dto.TrasactionTypeID == (int)TransactionType.Income)
				financialSafe!.Total = financialSafe.Total + dto.Total;
			context.Safe.Update(financialSafe!);

			// Subtract Pay Amount from Total of Client That we will want to collect ==> (-8000 + 5000 = -3000)

			clientDb.Total += dto.Total;
			context.Clients.Update(clientDb);

			await context.SaveChangesAsync();
			#endregion
			var cleint = await GetClientById((int)dto.Id!);
			response.ResponseID = 1;
			response.ResponseValue = cleint.ResponseValue;
			return response;
		}

		public async Task<RequestResponse<TransactionWithClientData>> GetTransactionsWithCleintData(int clientid)
		{
			var response = new RequestResponse<TransactionWithClientData> { ResponseID = 0, ResponseValue = new TransactionWithClientData() };
			var transactions = await context.Transactions.Where(c => c.ClientID == clientid).Include(c => c.Client).Include(c => c.TransactionProducts!).ThenInclude(c => c.Product).ToListAsync();
			if (transactions.Count != 0)
			{
				var transactionListDto = new List<TransactionMainDataDto>();

				foreach (var transaction in transactions)
				{
					var transactionDto = new TransactionMainDataDto();
					var productListDto = new List<ProductListDto>();


					transactionDto.ID = transaction.TransactionID;
					transactionDto.ClientID = transaction.ClientID;
					transactionDto.ClientName = transaction?.Client?.ClientName ?? "";
					transactionDto.DriverName = transaction?.DriverName ?? "";
					transactionDto.Date = transaction?.Created_Date ?? DateTime.Now.Date;
					transactionDto.Notes = transaction?.Notes ?? "";
					transactionDto.Price = transaction?.Price ?? 0;
					transactionDto.Total = transaction?.Total ?? 0;
					transactionDto.Payed = transaction?.Payed ?? 0;
					transactionDto.CarCapacity = transaction?.TotalCapcity ?? 0;
					transactionDto.Remaining = transaction?.Remaining ?? 0;
					transactionDto.TypeId = (int)TransactionType.Income;

					foreach (var item in transaction?.TransactionProducts)
					{
						var productDto = new ProductListDto();
						if (item.Qunatity != null)
						{
							productDto.ProductID = item.Product?.ProductID;
							productDto.ProductName = item.Product?.ProductName;
							productDto.Quantity = item.Qunatity;
						}
						if (item.Number != null)
						{
							productDto.ProductBoxName = item.Product?.ProductName;
							productDto.ProductBoxID = item.Product?.ProductID;
							productDto.Number = item.Number;
						}
						productListDto.Add(productDto);
					}
					transactionDto.ProductList = productListDto;

					transactionListDto.Add(transactionDto);
				}
				var client = await GetClientById(clientid);
				response.ResponseValue.Name = client.ResponseValue?.Name ?? "";
				response.ResponseValue.ID = client.ResponseValue?.ID ?? 0;
				response.ResponseValue.Date = client.ResponseValue?.Created_Date;
				response.ResponseValue.Total = client.ResponseValue?.Total ?? 0;
				response.ResponseValue.TransactionsList = transactionListDto;
				response.ResponseID = 1;
			}
			return response;
		}
	}
}
