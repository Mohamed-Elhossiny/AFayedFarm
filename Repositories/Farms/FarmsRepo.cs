using AFayedFarm.Dtos;
using AFayedFarm.Enums;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Repositories.Supplier
{
	public class FarmsRepo : IFarmsRepo
	{
		private readonly FarmContext context;

		public FarmsRepo(FarmContext context)
		{
			this.context = context;
		}
		public async Task<FarmDto> AddFarmAsync(AddFarmDto farmDto)
		{
			var farm = new FarmDto();
			var farmdb = context.Farms.Where(f => f.FarmsName.ToLower() == farmDto.Name.ToLower()).FirstOrDefault();
			if (farmdb == null)
			{
				var Farm = new Farms()
				{
					FarmsName = farmDto.Name,
					Create_Date = DateTime.Now.Date
				};
				await context.Farms.AddAsync(Farm);
				await context.SaveChangesAsync();
				farm.Name = Farm.FarmsName;
				farm.ID = Farm.FarmsID;
				farm.Total = Farm.TotalRemaining == null ? 0 : Farm.TotalRemaining;
				return farm;
			}

			farm.ID = 0;
			return farm;
		}

		public async Task<RequestResponse<FarmRecordDto>> AddFarmRecord(AddFarmRecordDto farmDto)
		{
			var response = new RequestResponse<FarmRecordDto> { ResponseID = 0 };
			var farmProduct = new FarmsProduct();
			if (farmDto != null)
			{
				var remaining = (farmDto.Total - farmDto.Paied);

				#region Add Transactions to Financial Safe

				var transaction = new SafeTransaction();
				transaction.SafeID = 2;
				transaction.FarmID = farmDto.FarmsID;
				transaction.TypeID = farmDto.TypeId;
				transaction.Type = ((TransactionType)farmDto.TypeId!).ToString();
				transaction.Total = -1 * farmDto.Paied;
				transaction.Notes = farmDto.FarmsNotes;
				transaction.IsfromRecord = true;

				await context.SafeTransactions.AddAsync(transaction);

				var financialSafe = await context.Safe.FindAsync(2);
				if (farmDto.TypeId == (int)TransactionType.Pay)
					financialSafe!.Total = financialSafe.Total - farmDto.Paied;

				context.Safe.Update(financialSafe!);

				var farmDb = await context.Farms.Where(f => f.FarmsID == farmDto.FarmsID).FirstOrDefaultAsync();
				if (farmDb!.TotalRemaining == null)
					farmDb.TotalRemaining = 0;
				farmDb!.TotalRemaining += remaining;
				context.Farms.Update(farmDb);

				await context.SaveChangesAsync();
				#endregion

				farmProduct.FarmsID = farmDto.FarmsID;
				farmProduct.ProductID = farmDto.ProductID;
				farmProduct.Quantity = farmDto.Quantity;
				farmProduct.NetQuantity = farmDto.NetQuantity;
				farmProduct.SupplyDate = farmDto.SupplyDate ?? DateTime.Now;
				farmProduct.Created_Date = DateTime.Now.Date;
				farmProduct.CarNumber = farmDto.CarNumber;
				farmProduct.Discount = farmDto.Discount;
				farmProduct.Price = farmDto.Price;
				farmProduct.TotalPrice = farmDto.Total;
				farmProduct.Paied = farmDto.Paied;
				farmProduct.FarmsNotes = farmDto.FarmsNotes;
				farmProduct.Remaining = remaining;
				farmProduct.isPercentage = farmDto.isPercentage;
				farmProduct.FinancialId = transaction.ID;
				
				await context.FarmsProducts.AddAsync(farmProduct);
				await context.SaveChangesAsync();

				await AddProductToStore(farmDto);

				var farmrecordDto = await GetFarmRecordByID(farmProduct.FarmProductID);
				//farmrecordDto.ResponseValue!.FinancialId = transaction.ID;

				response.ResponseID = 1;
				response.ResponseValue = farmrecordDto.ResponseValue;
			}
			return response;

		}

		public async Task<RequestResponse<List<FarmRecordDto>>> GetAllFarmRecords(int farmID)
		{
			var response = new RequestResponse<List<FarmRecordDto>>() { ResponseID = 0 };
			var farmsRecord = new List<FarmRecordDto>();
			var records = await context.FarmsProducts.Include(c => c.Farms).Include(c => c.Product).OrderByDescending(c => c.FarmProductID).Where(c => c.FarmsID == farmID).ToListAsync();
			var transactionRecordDb = await context.SafeTransactions.Include(c => c.Farm).Where(c => c.FarmID == farmID).ToListAsync();

			if (records.Count != 0 || transactionRecordDb.Count != 0)
			{
				decimal? remaining = 0;
				if (records.Count != 0)
				{
					foreach (var item in records)
					{
						remaining = item.TotalPrice - item.Paied;

						var record = new FarmRecordDto();
						record.FarmsID = item.Farms!.FarmsID;
						record.FarmsName = item.Farms.FarmsName;
						record.ProductID = item.Product!.ProductID;
						record.ProductName = item.Product.ProductName;
						record.SupplyDate = item.SupplyDate;
						record.Created_Date = item.Created_Date;
						record.Quantity = item.Quantity;
						record.Discount = item.Discount;
						record.NetQuantity = item.NetQuantity;
						record.Price = item.Price;
						record.Total = item.TotalPrice;
						record.Paied = item.Paied;
						record.Remaining = remaining;
						record.FarmsNotes = item.FarmsNotes;
						record.CarNumber = item.CarNumber;
						record.isPercentage = item.isPercentage;

						farmsRecord.Add(record);
					}
				}

				if (transactionRecordDb.Count != 0)
				{
					foreach (var item in transactionRecordDb)
					{
						var transactionRecord = new FarmRecordDto();
						transactionRecord.FarmsID = (int)item.Farm!.FarmsID;
						transactionRecord.FarmsName = item.Farm!.FarmsName;
						transactionRecord.Description = TransactionType.Pay.ToString();
						transactionRecord.Paied = -1 * item.Total;
						transactionRecord.FarmsNotes = item.Notes;
						transactionRecord.Created_Date = item.Created_Date;

						farmsRecord.Add(transactionRecord);
					}

				}

				response.ResponseID = 1;
				response.ResponseValue = farmsRecord;
				return response;

			}

			response.ResponseValue = farmsRecord;
			return response;
		}

		public async Task<RequestResponse<FarmRecordsWithTotalRemainingDto>> GetAllFarmRecordsWithTotal(int farmID)
		{
			var response = new RequestResponse<FarmRecordsWithTotalRemainingDto>() { ResponseID = 0, ResponseValue = new FarmRecordsWithTotalRemainingDto() };
			var farmsRecord = new List<FarmRecordDto>();
			var records = await context.FarmsProducts.Include(c => c.Farms).Include(c => c.Product).OrderByDescending(c => c.FarmProductID).Where(c => c.FarmsID == farmID).ToListAsync();
			var transactionRecordDb = await context.SafeTransactions.Include(c => c.Farm).Where(c => c.FarmID == farmID).ToListAsync();
			if (records.Count != 0 || transactionRecordDb.Count != 0)
			{
				if (records.Count != 0)
				{
					decimal? remaining = 0;
					foreach (var item in records)
					{
						remaining = item.TotalPrice - item.Paied;
						var record = new FarmRecordDto();
						record.FarmRecordID = item.FarmProductID;
						record.FarmsID = item.Farms.FarmsID;
						record.FarmsName = item.Farms.FarmsName;
						record.ProductID = item?.Product?.ProductID;
						record.ProductName = item?.Product?.ProductName;
						record.SupplyDate = item.SupplyDate.HasValue ? item.SupplyDate.Value.Date : DateTime.Now.Date;
						record.Created_Date = item.Created_Date.HasValue ? item.Created_Date.Value.Date : DateTime.Now.Date;
						record.Quantity = item?.Quantity;
						record.Discount = item?.Discount;
						record.NetQuantity = item?.NetQuantity;
						record.Price = item?.Price;
						record.Total = item?.TotalPrice;
						record.Paied = item?.Paied;
						record.Remaining = remaining;
						record.FarmsNotes = item?.FarmsNotes;
						record.CarNumber = item?.CarNumber;
						record.isPercentage = item.isPercentage;

						farmsRecord.Add(record);
					}
				}
				if (transactionRecordDb.Count != 0)
				{
					foreach (var item in transactionRecordDb)
					{
						var transactionRecord = new FarmRecordDto();
						transactionRecord.FarmsID = (int)item.Farm!.FarmsID;
						transactionRecord.FarmsName = item.Farm!.FarmsName;
						transactionRecord.Description = TransactionType.Pay.ToString();
						transactionRecord.Paied = -1 * item.Total;
						transactionRecord.FarmsNotes = item.Notes;

						farmsRecord.Add(transactionRecord);
					}
				}

				//var TotalRemaining = await GetTotalRemaining(farmID);
				var farmDb = await context.Farms.Where(f => f.FarmsID == farmID).FirstOrDefaultAsync();
				var TotalRemaining = farmDb!.TotalRemaining;
				response.ResponseValue.TotalRemaining = TotalRemaining;
				response.ResponseValue.FarmRecords = farmsRecord;
				response.ResponseID = 1;
				return response;
			}

			return response;
		}

		public async Task<FarmDto> GetFarmById(int id)
		{
			var farmDb = await context.Farms.FindAsync(id);
			//var total = await GetTotalRemaining(id);
			if (farmDb != null)
			{
				var Farm = new FarmDto()
				{
					Name = farmDb.FarmsName,
					ID = farmDb.FarmsID,
					Created_Date = DateOnly.FromDateTime(farmDb.Create_Date ?? DateTime.Now.Date),
					Total = farmDb.TotalRemaining == null ? 0 : farmDb.TotalRemaining
					//Total = total.ResponseValue
				};
				return Farm;
			}
			return new FarmDto { ID = 0 };
		}

		public async Task<FarmDto> GetFarmByName(string farmName)
		{
			var farm = new FarmDto();
			var farmdb = await context.Farms.Where(f => f.FarmsName.ToLower() == farmName.ToLower()).FirstOrDefaultAsync();
			if (farmdb != null)
			{
				var Farm = new FarmDto()
				{
					Name = farmdb.FarmsName,
					Created_Date = DateOnly.FromDateTime(farmdb.Create_Date ?? DateTime.Now),
					ID = farmdb.FarmsID,
					Total = farmdb.TotalRemaining == null ? 0 : farmdb.TotalRemaining
				};
				return Farm;
			}
			farm.ID = 0;
			return farm;
		}

		public async Task<RequestResponse<FarmRecordDto>> GetFarmRecordByID(int recordID)
		{
			var response = new RequestResponse<FarmRecordDto> { ResponseID = 0 };
			var record = new FarmRecordDto();
			var recordDb = await context.FarmsProducts
				.Include(c => c.Farms)
				.Include(c => c.Product)
				.Where(c => c.FarmProductID == recordID).FirstOrDefaultAsync();
			if (recordDb != null)
			{
				record.FarmRecordID = recordDb.FarmProductID;
				record.FarmsID = recordDb.Farms.FarmsID;
				record.FarmsName = recordDb.Farms.FarmsName;
				record.ProductID = recordDb?.Product?.ProductID;
				record.ProductName = recordDb?.Product?.ProductName;
				record.SupplyDate = recordDb.SupplyDate;
				record.Created_Date = recordDb.Created_Date.HasValue ? recordDb.Created_Date.Value.Date : DateTime.Now.Date;
				record.Quantity = recordDb.Quantity;
				record.Discount = recordDb.Discount;
				record.NetQuantity = recordDb.NetQuantity;
				record.Price = recordDb.Price;
				record.Total = recordDb.TotalPrice;
				record.Paied = recordDb.Paied;
				record.Remaining = recordDb.Remaining;
				record.FarmsNotes = recordDb.FarmsNotes;
				record.CarNumber = recordDb.CarNumber;
				record.isPercentage = recordDb.isPercentage;
				record.TypeId = 2;


				response.ResponseID = 1;
				response.ResponseValue = record;
			}
			return response;

		}

		public async Task<RequestResponse<FarmRecordsWithFarmDataDto>> GetFarmRecordWithFarmDataByID(int farmID, int currentPage, int pageSize)
		{
			var response = new RequestResponse<FarmRecordsWithFarmDataDto>() { ResponseID = 0, ResponseValue = new FarmRecordsWithFarmDataDto() };
			var farmsRecord = new List<FarmRecordDto>();
			var framRecords = await context.FarmsProducts
				.Include(c => c.Farms)
				.Include(c => c.Product)
				.OrderByDescending(c => c.Created_Date)
				.Where(c => c.FarmsID == farmID).ToListAsync();

			var records = framRecords.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			var transactionRecordDbs = await context.SafeTransactions
				.Include(c => c.Farm)
				.Where(c => c.FarmID == farmID && c.IsfromRecord == false).OrderByDescending(c => c.Created_Date).ToListAsync();

			var transactionRecordDb = transactionRecordDbs.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (records.Count != 0 || transactionRecordDb.Count != 0)
			{
				var index = 1;
				if (records.Count != 0)
				{
					decimal? remaining = 0;
					foreach (var item in records)
					{
						remaining = item.TotalPrice - item.Paied;
						var record = new FarmRecordDto();
						record.Id = index;
						record.FarmRecordID = item.FarmProductID;
						record.FarmsID = item.Farms!.FarmsID;
						record.FarmsName = item.Farms.FarmsName;
						record.ProductID = item.Product!.ProductID;
						record.ProductName = item.Product.ProductName;
						record.SupplyDate = item.SupplyDate;
						record.Created_Date = item.Created_Date.HasValue ? item.Created_Date.Value.Date : DateTime.Now.Date;
						record.Quantity = item.Quantity;
						record.Discount = item.Discount;
						record.NetQuantity = item.NetQuantity;
						record.Price = item.Price;
						record.Total = item.TotalPrice;
						record.Paied = item.Paied;
						record.Remaining = remaining;
						record.FarmsNotes = item.FarmsNotes;
						record.CarNumber = item.CarNumber;
						record.isPercentage = item.isPercentage;
						++index;

						farmsRecord.Add(record);
					}
				}
				if (transactionRecordDb.Count != 0)
				{
					foreach (var item in transactionRecordDb)
					{
						var transactionRecord = new FarmRecordDto();
						transactionRecord.Id = index;
						transactionRecord.FarmRecordID = item.ID;
						transactionRecord.FarmsID = (int)item.Farm!.FarmsID;
						transactionRecord.FarmsName = item.Farm!.FarmsName;
						transactionRecord.Description = TransactionType.Pay.ToString();
						transactionRecord.Paied = -1 * item.Total;
						transactionRecord.FarmsNotes = item.Notes;
						transactionRecord.Created_Date = item.Created_Date;
						++index;

						farmsRecord.Add(transactionRecord);
					}
				}
				var farmDataWithRecord = await GetFarmById(farmID);
				var farmDB = await context.Farms.Where(f => f.FarmsID == farmID).FirstOrDefaultAsync();
				var TotalRemainingWithRecord = farmDB!.TotalRemaining;

				var totalRecords = framRecords.Count() + transactionRecordDbs.Count();
				response.LastPage = (int)Math.Ceiling((double)totalRecords / pageSize);
				response.CurrentPage = currentPage;
				response.PageSize = pageSize;
				response.ResponseValue.Name = farmDataWithRecord.Name;
				response.ResponseValue.ID = farmDataWithRecord.ID;
				response.ResponseValue.Total = TotalRemainingWithRecord;
				response.ResponseValue.FarmRecords = farmsRecord;

				response.ResponseID = 1;
				return response;
			}

			var farmData = await GetFarmById(farmID);
			var TotalRemaining = await CalculateTotalRemainingFromRecords(farmID);

			response.ResponseValue.Name = farmData.Name;
			response.ResponseValue.ID = farmData.ID;
			response.ResponseValue.Date = farmData.Created_Date;
			response.ResponseValue.Total = TotalRemaining.ResponseValue;
			response.ResponseValue.FarmRecords = farmsRecord;
			return response;
		}

		public async Task<List<FarmDto>> GetFarmsAsync()
		{
			var AllFarms = new List<FarmDto>();
			var farmsDb = await context.Farms.OrderByDescending(c => c.Create_Date).Select(f => new FarmDto
			{
				ID = f.FarmsID,
				Name = f.FarmsName,
				Total = f.TotalRemaining,
				Created_Date = DateOnly.FromDateTime(f.Create_Date ?? DateTime.Now)
			}).ToListAsync();

			foreach (var item in farmsDb)
			{
				if (item.Total == null)
				{
					var remaning = await CalculateTotalRemainingFromRecords((int)item.ID!);
					item.Total = remaning.ResponseValue;
				}
				AllFarms.Add(item);
			}
			return AllFarms;
		}

		public async Task<RequestResponse<decimal>> CalculateTotalRemainingFromRecords(int farmsID)
		{
			var response = new RequestResponse<decimal> { ResponseID = 0, ResponseValue = 0 };
			var farmsList = await GetAllFarmRecords(farmsID);
			if (farmsList.ResponseID == 1)
			{
				decimal? total = 0;
				foreach (var item in farmsList.ResponseValue!)
				{
					if (item.Description == null)
						total += item.Remaining;
				}
				response.ResponseID = 1;
				response.ResponseValue = (decimal)total!;

				var farmDb = await context.Farms.Where(c => c.FarmsID == farmsID).FirstOrDefaultAsync();
				farmDb!.TotalRemaining = total;
				context.Farms.Update(farmDb);
				await context.SaveChangesAsync();
			}
			return response;

		}

		public async Task<RequestResponse<FarmDto>> UpdateFarm(int id, AddFarmDto farmDto)
		{
			var response = new RequestResponse<FarmDto> { ResponseID = 0, ResponseValue = new FarmDto() };

			var farmDb = await context.Farms.SingleOrDefaultAsync(m => m.FarmsID == id);
			if (farmDb != null)
			{
				farmDb.FarmsName = farmDto.Name;
				await context.SaveChangesAsync();

				var farm = await GetFarmById(id);

				response.ResponseID = 1;
				response.ResponseValue = farm;

			}

			return response;
		}

		public async Task<RequestResponse<FarmRecordDto>> UpdateFarmRecordAsync(int recordID, AddFarmRecordDto farmDto)
		{
			var response = new RequestResponse<FarmRecordDto>() { ResponseID = 0, ResponseValue = new FarmRecordDto() };

			var recordDb = await context.FarmsProducts
				.Include(c => c.Farms)
				.Include(c => c.Product)
				.Where(c => c.FarmProductID == recordID).FirstOrDefaultAsync();
			if (recordDb != null)
			{
				var incomeNetQty = farmDto.NetQuantity;
				if (farmDto.NetQuantity > recordDb.NetQuantity)
				{
					farmDto.NetQuantity = farmDto.NetQuantity - recordDb.NetQuantity;
					await AddProductToStore(farmDto);
				}
				if (recordDb.NetQuantity > farmDto.NetQuantity)
				{
					farmDto.NetQuantity = recordDb.NetQuantity - farmDto.NetQuantity;
					await RemoveProductFromStore(farmDto);
				}
				var transaction = new SafeTransaction();
				if (farmDto.TypeId == (int)TransactionType.Pay && farmDto.Paied != recordDb.Paied)
				{
					decimal? oldPayAmount = 0;
					decimal? incomePayed = farmDto.Paied;

					#region Remove Financial Record from database
					var financailReocrdDb = await context.SafeTransactions.Where(p => p.ID == recordDb.FinancialId).FirstOrDefaultAsync();
					if (financailReocrdDb != null)
					{
						oldPayAmount = -1 * financailReocrdDb.Total;

						financailReocrdDb.SafeID = 2;
						financailReocrdDb.FarmID = farmDto.FarmsID;
						financailReocrdDb.TypeID = farmDto.TypeId;
						financailReocrdDb.Type = farmDto.TypeId!= null ? ((TransactionType)farmDto.TypeId).ToString():TransactionType.Pay.ToString();
						financailReocrdDb.Total = -1 * farmDto.Paied;
						financailReocrdDb.Notes = farmDto.FarmsNotes;
						financailReocrdDb.Created_Date = DateTime.Now.Date;
						financailReocrdDb.IsfromRecord = true;

						context.SafeTransactions.Update(financailReocrdDb);
						await context.SaveChangesAsync();

						var safe = await context.Safe.FindAsync(2);
						safe!.Total += oldPayAmount;
						if (farmDto.TypeId == (int)TransactionType.Pay)
							safe!.Total = safe.Total - farmDto.Paied;

						context.Safe.Update(safe);

						await context.SaveChangesAsync();

					}
					#endregion

					var farmDb = await context.Farms.Where(f => f.FarmsID == recordDb.FarmsID).FirstOrDefaultAsync();
					if (farmDto.TypeId == (int)TransactionType.Pay && recordDb.Paied < incomePayed)
						farmDb!.TotalRemaining = farmDb.TotalRemaining - (incomePayed - recordDb.Paied);
					else
						farmDb!.TotalRemaining = farmDb.TotalRemaining + (recordDb.Paied - incomePayed );
					context.Farms.Update(farmDb!);

					await context.SaveChangesAsync();
					
				}

				var remaining = (farmDto.Total - farmDto.Paied);
				recordDb.FarmsID = farmDto.FarmsID;
				recordDb.ProductID = farmDto.ProductID;
				recordDb.Quantity = farmDto.Quantity;
				recordDb.NetQuantity = incomeNetQty;
				recordDb.SupplyDate = farmDto.SupplyDate != null ? farmDto.SupplyDate.Value.Date : null;
				recordDb.CarNumber = farmDto.CarNumber;
				recordDb.Discount = farmDto.Discount;
				recordDb.Price = farmDto.Price;
				recordDb.TotalPrice = farmDto.Total;
				recordDb.Paied = farmDto.Paied;
				recordDb.FarmsNotes = farmDto.FarmsNotes;
				recordDb.Remaining = remaining;
				recordDb.isPercentage = farmDto.isPercentage;
				recordDb.FinancialId = recordDb.FinancialId;

				context.FarmsProducts.Update(recordDb);
				await context.SaveChangesAsync();

				//await AddProductToStore(farmDto);

				var recordResult = await GetFarmRecordByID(recordID);
				if (recordResult.ResponseID == 1)
				{
					response.ResponseID = 1;
					response.ResponseValue = recordResult.ResponseValue;
					return response;
				}

			}
			return response;

		}

		public async Task<RequestResponse<bool>> AddProductToStore(AddFarmRecordDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			var productInStore = await context.StoreProducts.Where(c => c.ProductID == dto.ProductID).FirstOrDefaultAsync();
			if (productInStore != null)
			{
				if (productInStore.Quantity == null)
					productInStore.Quantity = 0;
				productInStore.Quantity += dto.NetQuantity;
				context.StoreProducts.Update(productInStore);
				await context.SaveChangesAsync();
			}
			else
			{
				var storeProduct = new StoreProduct();
				if (dto != null)
				{
					storeProduct.ProductID = dto.ProductID;
					storeProduct.StoreID = 2;
					storeProduct.Created_Date = DateTime.Now.Date;
					storeProduct.Quantity = dto.NetQuantity;

					await context.StoreProducts.AddAsync(storeProduct);
					await context.SaveChangesAsync();
				}
			}
			response.ResponseID = 1;
			response.ResponseValue = true;
			return response;
		}

		public async Task<RequestResponse<bool>> RemoveProductFromStore(AddFarmRecordDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			var productInStore = await context.StoreProducts.Where(c => c.ProductID == dto.ProductID).FirstOrDefaultAsync();
			if (productInStore != null)
			{
				if (productInStore.Quantity == null)
					productInStore.Quantity = 0;
				productInStore.Quantity -= dto.NetQuantity;
				context.StoreProducts.Update(productInStore);
				await context.SaveChangesAsync();

				response.ResponseID = 1;
				response.ResponseValue = true;
			}
			return response;
		}

		public async Task<RequestResponse<List<FarmRecordWithoutDescriptionDto>>> GetProductsDetails(int pageNumber, int pageSize)
		{
			var response = new RequestResponse<List<FarmRecordWithoutDescriptionDto>>() { ResponseID = 0 };
			var farmsRecord = new List<FarmRecordWithoutDescriptionDto>();

			var farmRecords = await context.FarmsProducts
				.Include(c => c.Farms)
				.Include(c => c.Product)
				.OrderByDescending(c => c.Created_Date).ToListAsync();

			var records = farmRecords.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			if (records.Count != 0)
			{
				//decimal? remaining = 0;
				foreach (var item in records)
				{
					//remaining = item.TotalPrice - item.Paied;
					var record = new FarmRecordWithoutDescriptionDto();
					record.FarmRecordID = item.FarmProductID;
					record.FarmsID = (int)(item.Farms?.FarmsID ?? 0);
					record.FarmsName = item.Farms?.FarmsName;
					record.ProductID = item.Product?.ProductID;
					record.ProductName = item.Product?.ProductName;
					record.SupplyDate = item.SupplyDate;
					record.Created_Date = item.Created_Date.HasValue ? item.Created_Date.Value.Date : DateTime.Now.Date;
					record.Quantity = item.Quantity;
					record.Discount = item.Discount;
					record.NetQuantity = item.NetQuantity;
					record.Price = item.Price;
					record.Total = item.TotalPrice;
					record.Paied = item.Paied;
					record.Remaining = item.Remaining;
					record.FarmsNotes = item.FarmsNotes;
					record.CarNumber = item.CarNumber;
					record.isPercentage = item.isPercentage;

					farmsRecord.Add(record);
				}
				response.ResponseID = 1;
				response.ResponseValue = farmsRecord;
				return response;
			}
			response.ResponseValue = farmsRecord;
			return response;
		}

		public async Task<RequestResponse<FarmDto>> PayToFarm(FarmPaymentDto dto)
		{
			var response = new RequestResponse<FarmDto> { ResponseID = 0, ResponseValue = new FarmDto() };
			var farmDb = await context.Farms.Where(e => e.FarmsID == dto.Id).FirstOrDefaultAsync();
			if (farmDb == null)
				return response;

			#region Add Transactions to Financial Safe
			var transaction = new SafeTransaction()
			{
				SafeID = 2,
				FarmID = dto.Id,
				TypeID = dto.TrasactionTypeID,
				Type = ((TransactionType)dto.TrasactionTypeID!).ToString(),
				Total = -1 * dto.Total,
				Notes = dto.Notes,
				Created_Date = DateTime.Now,
				IsfromRecord = false
			};
			await context.SafeTransactions.AddAsync(transaction);

			var financialSafe = await context.Safe.FindAsync(2);
			if (dto.TrasactionTypeID == (int)TransactionType.Pay)
				financialSafe!.Total = financialSafe.Total - dto.Total;
			context.Safe.Update(financialSafe!);

			// Subtract Pay Amount from Total Remaining of Fram
			farmDb.TotalRemaining -= dto.Total;
			context.Farms.Update(farmDb);

			await context.SaveChangesAsync();
			#endregion
			var farm = await GetFarmById((int)dto.Id!);
			response.ResponseID = 1;
			response.ResponseValue = farm;
			return response;
		}


	}
}
