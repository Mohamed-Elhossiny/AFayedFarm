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
				client.Total = 0;
				client.Created_Date = DateOnly.FromDateTime(Client.Create_Date.Value);
				return client;
			}

			client.ID = 0;
			return client;
		}

		public async Task<RequestResponse<TransactionMainDataDto>> GetTransactionByRecordId(int recordid)
		{
			var response = new RequestResponse<TransactionMainDataDto> { ResponseID = 0, ResponseValue = new TransactionMainDataDto() };
			var transaction = await context.Transactions.Where(c => c.TransactionID == recordid)
				.Include(c => c.Client)
				.Include(c => c.TransactionProducts!)
				.ThenInclude(c => c.Product).FirstOrDefaultAsync();
			if (transaction != null)
			{
				var transactionDto = new TransactionMainDataDto();

				var productListDto = new List<ProductListDto>();

				transactionDto.ID = transaction.TransactionID;
				transactionDto.ClientID = transaction.ClientID;
				transactionDto.ClientName = transaction?.Client?.ClientName ?? "";
				transactionDto.DriverName = transaction?.DriverName ?? "";
				transactionDto.Date = transaction?.ShippingDate ?? DateTime.Now.Date;
				transactionDto.PayDate = transaction?.PayDate ?? DateTime.Now.Date;
				transactionDto.CreatedDate = transaction?.Created_Date ?? DateTime.Now.Date;
				transactionDto.Total = transaction?.Total ?? 0;
				transactionDto.Payed = transaction?.Payed ?? 0;
				transactionDto.Remaining = transaction?.Remaining ?? 0;
				transactionDto.DeliveredToDriver = transaction?.DeliveredToDriver ?? 0;
				transactionDto.CarCapacity = transaction?.TotalCapcity ?? 0;
				transactionDto.Notes = transaction?.Notes ?? "";
				transactionDto.TypeId = (int)TransactionType.Income;

				foreach (var item in transaction?.TransactionProducts)
				{
					var productDto = new ProductListDto();
					if (item.Qunatity != null || item.Number != null)
					{
						productDto.Id = item.ID;
						productDto.ProductID = item.Product?.ProductID;
						productDto.ProductName = item.Product?.ProductName;
						productDto.Quantity = item.Qunatity;
						productDto.ProductBoxID = item.ProductBoxID;
						productDto.ProductBoxName = item.ProductBoxID.HasValue ?
							await context.Products.Where(p => p.ProductID == item.ProductBoxID.Value).Select(p => p.ProductName).FirstOrDefaultAsync() : "";
						productDto.Number = item.Number;
						productDto.Total = item.ProductTotal;
						productDto.Price = item.Price;
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
			transaction.PayDate = (DateTime)dto.PayDate!;
			transaction.Total = dto.Total;
			transaction.Payed = dto.Payed;
			transaction.Remaining = (dto.Total - dto.Payed);
			transaction.DriverName = dto.DriverName;
			transaction.DeliveredToDriver = dto.DeliveredToDriver;
			transaction.TotalCapcity = dto.CarCapacity;
			transaction.Notes = dto.Notes;

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
						transactionProduct.ProductBoxID = item.ProductBoxID;
						transactionProduct.Number = item.Number;
						transactionProduct.Price = item.Price;
						transactionProduct.ProductTotal = item.Total;

						await context.TransactionProducts.AddAsync(transactionProduct);
						await context.SaveChangesAsync();

					}

				}

				await UpdateProductQuantityInStore(dto);
			}

			#region Add Transactions to Financial Safe

			var safeTransaction = new SafeTransaction();
			safeTransaction.SafeID = 2;
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
			if (clientDb!.Total == null)
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
				Total = c.Total ?? 0,
				Created_Date = DateOnly.FromDateTime(c.Create_Date ?? DateTime.Now)
			}).OrderByDescending(c => c.ID).ToListAsync();
			if (clientsDb.Count() != 0)
				return clientsDb;
			else
				return new List<ClientDto>();
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
					Total = clientDb.Total ?? 0,
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
							productDto.Total = item.ProductTotal;
							productDto.Price = item.Price;
						}
						if (item.Number != null)
						{
							productDto.ProductBoxName = item.Product?.ProductName;
							productDto.ProductBoxID = item.Product?.ProductID;
							productDto.Number = item.Number;
							productDto.Total = item.ProductTotal;
							productDto.Price = item.Price;
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
				SafeID = 2,
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

		public async Task<RequestResponse<TransactionWithClientData>> GetTransactionsWithCleintData(int clientid, int currentPage, int pageSize)
		{
			var response = new RequestResponse<TransactionWithClientData> { ResponseID = 0, ResponseValue = new TransactionWithClientData() { TransactionsList = new() } };
			var transactionsDb = await context.Transactions
				.Where(c => c.ClientID == clientid)
				.Include(c => c.Client)
				.Include(c => c.TransactionProducts!).ThenInclude(c => c.Product)
				.OrderByDescending(c => c.Created_Date).ToListAsync();

			var transactions = transactionsDb.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			var financialtransactionRecordDbs = await context.SafeTransactions
				.Include(c => c.Client)
				.Where(c => c.CLientID == clientid).ToListAsync();

			var financialtransactionRecordDb = financialtransactionRecordDbs.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (transactions.Count != 0 || financialtransactionRecordDb.Count != 0)
			{
				var transactionListDto = new List<TransactionMainDataDto>();

				if (transactions.Count != 0)
				{

					foreach (var transaction in transactions)
					{
						var transactionDto = new TransactionMainDataDto();
						var productListDto = new List<ProductListDto>();

						transactionDto.ID = transaction.TransactionID;
						transactionDto.ClientID = transaction.ClientID;
						transactionDto.ClientName = transaction?.Client?.ClientName ?? "";
						transactionDto.DriverName = transaction?.DriverName ?? "";
						transactionDto.Date = transaction?.ShippingDate ?? DateTime.Now.Date;
						transactionDto.PayDate = transaction?.PayDate ?? DateTime.Now.Date;
						transactionDto.CreatedDate = transaction?.Created_Date ?? DateTime.Now.Date;
						transactionDto.Notes = transaction?.Notes ?? "";
						transactionDto.Total = transaction?.Total ?? 0;
						transactionDto.Payed = transaction?.Payed ?? 0;
						transactionDto.CarCapacity = transaction?.TotalCapcity ?? 0;
						transactionDto.Remaining = transaction?.Remaining ?? 0;
						transactionDto.DeliveredToDriver = transaction?.DeliveredToDriver ?? 0;
						transactionDto.TypeId = (int)TransactionType.Income;

						foreach (var item in transaction?.TransactionProducts)
						{
							var productDto = new ProductListDto();

							if (item.Qunatity != 0 || item.Number != null)
							{
								productDto.Id = item.ID;
								productDto.ProductID = item.Product?.ProductID;
								productDto.ProductName = item.Product?.ProductName;
								productDto.Quantity = item.Qunatity;
								productDto.ProductBoxID = item.ProductBoxID;
								productDto.ProductBoxName = item.ProductBoxID.HasValue ?
									await context.Products.Where(p => p.ProductID == item.ProductBoxID.Value).Select(p => p.ProductName).FirstOrDefaultAsync() : "";
								productDto.Number = item.Number;
								productDto.Total = item.ProductTotal;
								productDto.Price = item.Price;
							}

							productListDto.Add(productDto);
						}
						transactionDto.ProductList = productListDto;

						transactionListDto.Add(transactionDto);
					}
				}

				if (financialtransactionRecordDb.Count != 0)
				{
					foreach (var item in financialtransactionRecordDb)
					{
						var financialtransactionRecord = new TransactionMainDataDto();
						financialtransactionRecord.ID = item.ID;
						financialtransactionRecord.ClientID = (int)item.Client!.ClientID;
						financialtransactionRecord.ClientName = item.Client!.ClientName;
						financialtransactionRecord.Description = TransactionType.Income.ToString();
						financialtransactionRecord.Payed = item.Total;
						financialtransactionRecord.Notes = item.Notes;
						financialtransactionRecord.CreatedDate = item.Created_Date;

						transactionListDto.Add(financialtransactionRecord);
					}
				}

				var totalRecords = transactionsDb.Count() + financialtransactionRecordDbs.Count();
				response.LastPage = (int)Math.Ceiling((double)totalRecords / pageSize);
				response.CurrentPage = currentPage;
				response.PageSize = pageSize;

				response.ResponseValue.TransactionsList = transactionListDto;
				response.ResponseID = 1;
			}
			var client = await GetClientById(clientid);
			response.ResponseValue.Name = client.ResponseValue?.Name ?? "";
			response.ResponseValue.ID = client.ResponseValue?.ID ?? 0;
			response.ResponseValue.Date = client.ResponseValue?.Created_Date;
			response.ResponseValue.Total = client.ResponseValue?.Total ?? 0;
			return response;
		}

		public async Task<RequestResponse<TransactionMainDataDto>> UpdateClientRecord(int id, AddTransactionMainDataDto dto)
		{
			var response = new RequestResponse<TransactionMainDataDto> { ResponseID = 0, ResponseValue = new TransactionMainDataDto() };
			try
			{
				var transaction = await context.Transactions.Where(c => c.TransactionID == id)
					.Include(c => c.Client)
					.Include(c => c.TransactionProducts!)
					.ThenInclude(c => c.Product).FirstOrDefaultAsync();

				if (transaction != null)
				{
					transaction.ClientID = (int)dto.ClientID!;
					transaction.ShippingDate = (DateTime)dto.Date!;
					transaction.PayDate = (DateTime)dto.PayDate!;
					transaction.Total = dto.Total;
					transaction.Payed = dto.Payed;
					transaction.Remaining = (dto.Total - dto.Payed);
					transaction.DriverName = dto.DriverName;
					transaction.DeliveredToDriver = dto.DeliveredToDriver;
					transaction.TotalCapcity = dto.CarCapacity;
					transaction.Notes = dto.Notes;

					context.Transactions.Update(transaction);
					await context.SaveChangesAsync();

					/// Asscoiate Product List to This Transaction
					/// 
					var productListDb = await context.TransactionProducts
						.Where(c => c.TransactionID == transaction.TransactionID)
						.Include(c => c.Product).ToListAsync();

					if (productListDb.Count != 0 && dto.ProductList.Count != 0)
					{

						foreach (var item in dto.ProductList)
						{
							var existingProduct = productListDb.FirstOrDefault(p => p.ProductID == item.ProductID);
							if (existingProduct != null)
							{
								var inComeQuantity = item.Quantity;
								var inComeNumber = item.Number;

								if (inComeQuantity > existingProduct.Qunatity || inComeNumber > existingProduct.Number)
								{
									await UpdateProductQuantityInStore(dto);
								}

								if (inComeQuantity < existingProduct.Qunatity)
								{
									var newProductList = new AddProductListDto()
									{
										ProductID = item.ProductID,
										Quantity = item.Quantity,
										Price = item.Price,
										Total = item.Total
									};
									await ReturnProductToStore(newProductList);
								}

								if (inComeNumber < existingProduct.Number)
								{
									var newProductList = new AddProductListDto()
									{
										ProductID = item.ProductBoxID,
										Number = item.Number,
										Price = item.Price,
										Total = item.Total
									};
									await ReturnProductBoxToStore(newProductList);
								}

								existingProduct.TransactionID = transaction.TransactionID;
								existingProduct.ProductID = item.ProductID;
								existingProduct.ProductBoxID = item.ProductBoxID;
								existingProduct.Number = item.Number;
								existingProduct.Qunatity = item.Quantity;
								existingProduct.ProductTotal = item.Total;
								existingProduct.Price = item.Price;

								context.TransactionProducts.Update(existingProduct);
								await context.SaveChangesAsync();



							}

						}
					}

					if (dto.ProductList.Count() > productListDb.Count())
					{
						foreach (var item in dto.ProductList)
						{
							if (item.ProductID != 0)
							{
								var transactionProduct = new TransactionProduct();
								transactionProduct.TransactionID = transaction.TransactionID;
								transactionProduct.ProductID = item.ProductID;
								transactionProduct.Qunatity = item.Quantity;
								transactionProduct.ProductBoxID = item.ProductBoxID;
								transactionProduct.Number = item.Number;
								transactionProduct.Price = item.Price;
								transactionProduct.ProductTotal = item.Total;

								await context.TransactionProducts.AddAsync(transactionProduct);
								await context.SaveChangesAsync();

							}

						}

						await UpdateProductQuantityInStore(dto);
					}

					#region Safe Transaction

					if (dto.TypeId == (int)TransactionType.Income && dto.Payed != transaction.Payed)
					{

						var safeTransaction = new SafeTransaction();
						safeTransaction.SafeID = 2;
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


						var clientDb = await context.Clients.Where(f => f.ClientID == dto.ClientID).FirstOrDefaultAsync();
						if (clientDb!.Total == null)
							clientDb.Total = 0;
						clientDb!.Total += -1 * (dto.Total - dto.Payed);
						context.Clients.Update(clientDb);

						await context.SaveChangesAsync();

					}
					#endregion
					// TO DO ==> Get Transaction Record

					var transactionDto = await GetTransactionByRecordId(transaction.TransactionID);
					if (transactionDto.ResponseID == 1)
					{
						response.ResponseID = 1;
						response.ResponseValue = transactionDto.ResponseValue;
						return response;
					}

				}


			}
			catch (Exception ex)
			{
				response.ResponseID = 0;
				response.ResponseValue = new TransactionMainDataDto();
			}
			return response;

		}

		public async Task<RequestResponse<bool>> ReturnProductToStore(AddProductListDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			var productInStore = await context.StoreProducts.Where(c => c.ProductID == dto.ProductID).FirstOrDefaultAsync();
			if (productInStore != null)
			{
				if (productInStore.Quantity == null)
					productInStore.Quantity = 0;
				productInStore.Quantity += dto.Quantity;
				context.StoreProducts.Update(productInStore);
				await context.SaveChangesAsync();
				response.ResponseID = 1;
				response.ResponseValue = true;
			}
			return response;
		}

		public async Task<RequestResponse<bool>> ReturnProductBoxToStore(AddProductListDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			var productBoxInStore = await context.StoreProducts.Where(c => c.ProductID == dto.ProductBoxID).FirstOrDefaultAsync();
			if (productBoxInStore != null)
			{
				if (productBoxInStore.Quantity == null)
					productBoxInStore.Quantity = 0;
				productBoxInStore.Quantity += dto.Number;
				context.StoreProducts.Update(productBoxInStore);
				await context.SaveChangesAsync();
				response.ResponseID = 1;
				response.ResponseValue = true;
			}
			return response;
		}
	}
}
