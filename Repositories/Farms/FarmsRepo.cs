using AFayedFarm.Dtos;
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
				};
				await context.Farms.AddAsync(Farm);
				await context.SaveChangesAsync();
				farm.Name = Farm.FarmsName;
				farm.ID = Farm.FarmsID;
				return farm;
			}

			farm.ID = 0;
			return farm;
		}

		public async Task<RequestResponse<bool>> AddFarmRecord(AddFarmRecordDto farmDto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0 };
			var farmProduct = new FarmsProduct();
			if (farmDto != null)
			{
				var remaining = (farmDto.Total - farmDto.Paied);

				farmProduct.FarmsID = farmDto.FarmsID;
				farmProduct.ProductID = farmDto.ProductID;
				farmProduct.Quantity = farmDto.Quantity;
				farmProduct.SupplyDate = farmDto.SupplyDate ?? DateTime.Now;
				farmProduct.Created_Date = farmDto.Created_Date ?? DateTime.Now;
				farmProduct.CarNumber = farmDto.CarNumber;
				farmProduct.Discount = farmDto.Discount;
				farmProduct.Price = farmDto.Price;
				farmProduct.TotalPrice = farmDto.Total;
				farmProduct.Paied = farmDto.Paied;
				farmProduct.FarmsNotes = farmDto.FarmsNotes;
				farmProduct.Remaining = remaining;
				await context.FarmsProducts.AddAsync(farmProduct);
				await context.SaveChangesAsync();
				response.ResponseID = 1;

				await AddProductToStore(farmDto);

			}
			return response;

		}

		public async Task<RequestResponse<List<FarmRecordDto>>> GetAllFarmRecords(int farmID)
		{
			var response = new RequestResponse<List<FarmRecordDto>>() { ResponseID = 0 };
			var farmsRecord = new List<FarmRecordDto>();
			var records = await context.FarmsProducts.Include(c => c.Farms).Include(c => c.Product).Where(c => c.FarmsID == farmID).ToListAsync();
			if (records.Count != 0)
			{
				decimal? remaining = 0;
				foreach (var item in records)
				{
					remaining = item.TotalPrice - item.Paied;
					var record = new FarmRecordDto();
					record.FarmsID = item.Farms.FarmsID;
					record.FarmsName = item.Farms.FarmsName;
					record.ProductID = item.Product.ProductID;
					record.ProductName = item.Product.ProductName;
					record.SupplyDate = item.SupplyDate;
					record.Quantity = item.Quantity;
					record.Discount = item.Discount;
					record.NetQuantity = item.NetQuantity;
					record.Price = item.Price;
					record.Total = item.TotalPrice;
					record.Paied = item.Paied;
					record.Remaining = remaining;
					record.FarmsNotes = item.FarmsNotes;
					record.CarNumber = item.CarNumber;

					farmsRecord.Add(record);
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
			var records = await context.FarmsProducts.Include(c => c.Farms).Include(c => c.Product).Where(c => c.FarmsID == farmID).ToListAsync();
			if (records.Count != 0)
			{
				decimal? remaining = 0;
				foreach (var item in records)
				{
					remaining = item.TotalPrice - item.Paied;
					var record = new FarmRecordDto();
					record.FarmRecordID = item.FarmProductID;
					record.FarmsID =item.Farms.FarmsID;
					record.FarmsName = item.Farms.FarmsName;
					record.ProductID = item?.Product?.ProductID;
					record.ProductName = item?.Product?.ProductName;
					record.SupplyDate = item?.SupplyDate;
					record.Quantity = item?.Quantity;
					record.Discount = item?.Discount;
					record.NetQuantity = item?.NetQuantity;
					record.Price = item?.Price;
					record.Total = item?.TotalPrice;
					record.Paied = item?.Paied;
					record.Remaining = remaining;
					record.FarmsNotes = item?.FarmsNotes;
					record.CarNumber = item?.CarNumber;

					farmsRecord.Add(record);
				}

				var TotalRemaining = await GetTotalRemaining(farmID);
				response.ResponseValue.TotalRemaining = TotalRemaining.ResponseValue;
				response.ResponseValue.FarmRecords = farmsRecord;
				response.ResponseID = 1;
				return response;
			}

			return response;
		}

		public async Task<FarmDto> GetFarmById(int id)
		{
			var farmDb = await context.Farms.FindAsync(id);
			var total = await GetTotalRemaining(id);
			if (farmDb != null)
			{
				var Farm = new FarmDto()
				{
					Name = farmDb.FarmsName,
					ID = farmDb.FarmsID,
					Total = total.ResponseValue
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
					ID = farmdb.FarmsID,
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
			var recordDb = await context.FarmsProducts.Include(c => c.Farms).Include(c => c.Product).Where(c => c.FarmProductID == recordID).FirstOrDefaultAsync();
			if (recordDb != null)
			{
				record.FarmRecordID = recordDb.FarmProductID;
				record.FarmsID = recordDb.Farms.FarmsID;
				record.FarmsName = recordDb.Farms.FarmsName;
				record.ProductID = recordDb?.Product?.ProductID;
				record.ProductName = recordDb?.Product?.ProductName;
				record.SupplyDate = recordDb.SupplyDate;
				record.Quantity = recordDb.Quantity;
				record.Discount = recordDb.Discount;
				record.NetQuantity = recordDb.NetQuantity;
				record.Price = recordDb.Price;
				record.Total = recordDb.TotalPrice;
				record.Paied = recordDb.Paied;
				record.Remaining = recordDb.Remaining;
				record.FarmsNotes = recordDb.FarmsNotes;
				record.CarNumber = recordDb.CarNumber;

				response.ResponseID = 1;
				response.ResponseValue = record;
			}
			return response;

		}

		public async Task<RequestResponse<FarmRecordsWithFarmDataCto>> GetFarmRecordWithFarmDataByID(int farmID)
		{
			var response = new RequestResponse<FarmRecordsWithFarmDataCto>() { ResponseID = 0, ResponseValue = new FarmRecordsWithFarmDataCto() };
			var farmsRecord = new List<FarmRecordDto>();
			var records = await context.FarmsProducts.Include(c => c.Farms).Include(c => c.Product).Where(c => c.FarmsID == farmID).ToListAsync();
			if (records.Count != 0)
			{
				decimal? remaining = 0;
				foreach (var item in records)
				{
					remaining = item.TotalPrice - item.Paied;
					var record = new FarmRecordDto();
					record.FarmsID = item.Farms.FarmsID;
					record.FarmsName = item.Farms.FarmsName;
					record.ProductID = item.Product.ProductID;
					record.ProductName = item.Product.ProductName;
					record.SupplyDate = item.SupplyDate;
					record.Quantity = item.Quantity;
					record.Discount = item.Discount;
					record.NetQuantity = item.NetQuantity;
					record.Price = item.Price;
					record.Total = item.TotalPrice;
					record.Paied = item.Paied;
					record.Remaining = remaining;
					record.FarmsNotes = item.FarmsNotes;
					record.CarNumber = item.CarNumber;

					farmsRecord.Add(record);
				}
				var farmData = await GetFarmById(farmID);
				var TotalRemaining = await GetTotalRemaining(farmID);

				response.ResponseValue.Name = farmData.Name;
				response.ResponseValue.ID = farmData.ID;
				response.ResponseValue.Total = TotalRemaining.ResponseValue;
				response.ResponseValue.FarmRecords = farmsRecord;

				response.ResponseID = 1;
				return response;
			}
			return response;
		}

		public async Task<List<FarmDto>> GetFarmsAsync()
		{
			var AllFarms = new List<FarmDto>();
			var farmsDb = await context.Farms.Select(f => new FarmDto
			{
				ID = f.FarmsID,
				Name = f.FarmsName
			}).ToListAsync();

			foreach (var item in farmsDb)
			{
				var remaning = await GetTotalRemaining((int)item.ID);
				item.Total = remaning.ResponseValue;
				AllFarms.Add(item);
			}
			return AllFarms;
		}

		public async Task<RequestResponse<decimal>> GetTotalRemaining(int farmsID)
		{
			var response = new RequestResponse<decimal> { ResponseID = 0, ResponseValue = 0 };
			var farmsList = await GetAllFarmRecords(farmsID);
			if (farmsList.ResponseID == 1)
			{
				decimal? total = 0;
				foreach (var item in farmsList.ResponseValue)
				{
					total += item.Remaining;
				}
				response.ResponseID = 1;
				response.ResponseValue = (decimal)total;
			}
			return response;

		}

		public async Task<FarmDto> UpdateFarm(int id, AddFarmDto farmDto)
		{
			var farmDb = await context.Farms.SingleOrDefaultAsync(m => m.FarmsID == id);
			farmDb.FarmsName = farmDto.Name;
			await context.SaveChangesAsync();

			return new FarmDto { ID = farmDb.FarmsID, Name = farmDb.FarmsName };
		}

		public async Task<RequestResponse<FarmRecordDto>> UpdateFarmRecordAsync(int recordID, AddFarmRecordDto farmDto)
		{
			var response = new RequestResponse<FarmRecordDto>() { ResponseID = 0, ResponseValue = new FarmRecordDto() };

			var recordDb = await context.FarmsProducts.Include(c => c.Farms).Include(c => c.Product).Where(c => c.FarmProductID == recordID).FirstOrDefaultAsync();
			if (recordDb != null)
			{
				var remaining = (farmDto.Total - farmDto.Paied);

				recordDb.FarmsID = farmDto.FarmsID;
				recordDb.ProductID = farmDto.ProductID;
				recordDb.Quantity = farmDto.Quantity;
				recordDb.NetQuantity = farmDto.NetQuantity;
				recordDb.SupplyDate = farmDto.SupplyDate.Value != null ? farmDto.SupplyDate.Value.Date : null;
				recordDb.Created_Date = farmDto.Created_Date.Value != null ? farmDto.Created_Date.Value.Date : null;
				recordDb.CarNumber = farmDto.CarNumber;
				recordDb.Discount = farmDto.Discount;
				recordDb.Price = farmDto.Price;
				recordDb.TotalPrice = farmDto.Total;
				recordDb.Paied = farmDto.Paied;
				recordDb.FarmsNotes = farmDto.FarmsNotes;
				recordDb.Remaining = remaining;

				context.FarmsProducts.Update(recordDb);
				await context.SaveChangesAsync();

				await AddProductToStore(farmDto);

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
					storeProduct.StoreID = 1;
					storeProduct.Created_Date = dto.Created_Date;
					storeProduct.Quantity = dto.NetQuantity;

					await context.StoreProducts.AddAsync(storeProduct);
					await context.SaveChangesAsync();
				}
			}
			response.ResponseID = 1;
			response.ResponseValue = true;
			return response;
		}
	}
}
