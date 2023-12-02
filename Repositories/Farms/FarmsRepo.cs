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
			var farmdb = context.Farms.Where(f => f.FarmsName.ToLower() == farmDto.FarmName.ToLower()).FirstOrDefault();
			if (farmdb == null)
			{
				var Farm = new Farms()
				{
					FarmsName = farmDto.FarmName,
				};
				await context.Farms.AddAsync(Farm);
				await context.SaveChangesAsync();
				farm.FarmName = Farm.FarmsName;
				farm.FarmID = Farm.FarmsID;
				return farm;
			}

			farm.FarmID = 0;
			return farm;
		}

		public async Task<RequestResponse<bool>> AddFarmRecord(AddFarmRecordDto farmDto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0 };
			var farmProduct = new FarmsProduct();
			if (farmDto != null)
			{
				var remaining = (farmDto.TotalPrice - farmDto.Paied);

				farmProduct.FarmsID = farmDto.FarmsID;
				farmProduct.ProductID = farmDto.ProductID;
				farmProduct.Quantity = farmDto.Quantity;
				farmProduct.SupplyDate = farmDto.SupplyDate.Value != null ? farmDto.SupplyDate.Value.Date : null;
				farmProduct.Created_Date = farmDto.Created_Date.Value != null ? farmDto.Created_Date.Value.Date : null;
				farmProduct.CarNumber = farmDto.CarNumber;
				farmProduct.Discount = farmDto.Discount;
				farmProduct.Price = farmDto.Price;
				farmProduct.TotalPrice = farmDto.TotalPrice;
				farmProduct.Paied = farmDto.Paied;
				farmProduct.FarmsNotes = farmDto.FarmsNotes;
				farmProduct.Remaining = remaining;
				await context.FarmsProducts.AddAsync(farmProduct);
				await context.SaveChangesAsync();
				response.ResponseID = 1;
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
				foreach (var item in records)
				{
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
					record.TotalPrice = item.TotalPrice;
					record.Paied = item.Paied;

					farmsRecord.Add(record);
				}
				response.ResponseID = 1;
				response.ResponseValue = farmsRecord;
				return response;
			}
			response.ResponseValue = farmsRecord;
			return response;
		}

		public async Task<FarmDto> GetFarmById(int id)
		{
			var farmDb = await context.Farms.FindAsync(id);
			if (farmDb != null)
			{
				var Farm = new FarmDto()
				{
					FarmName = farmDb.FarmsName,
					FarmID = farmDb.FarmsID,
				};
				return Farm;
			}
			return new FarmDto { FarmID = 0 };
		}

		public async Task<FarmDto> GetFarmByName(string farmName)
		{
			var farm = new FarmDto();
			var farmdb = await context.Farms.Where(f => f.FarmsName.ToLower() == farmName.ToLower()).FirstOrDefaultAsync();
			if (farmdb != null)
			{
				var Farm = new FarmDto()
				{
					FarmName = farmdb.FarmsName,
					FarmID = farmdb.FarmsID,
				};
				return Farm;
			}
			farm.FarmID = 0;
			return farm;
		}

		public async Task<List<FarmDto>> GetFarmsAsync()
		{
			var farmsDb = await context.Farms.Select(f => new FarmDto
			{
				FarmID = f.FarmsID,
				FarmName = f.FarmsName
			}).ToListAsync();
			return farmsDb;
		}

		public async Task<FarmDto> UpdateFarm(int id, AddFarmDto farmDto)
		{
			var farmDb = await context.Farms.SingleOrDefaultAsync(m => m.FarmsID == id);
			farmDb.FarmsName = farmDto.FarmName;
			await context.SaveChangesAsync();

			return new FarmDto { FarmID = farmDb.FarmsID, FarmName = farmDb.FarmsName };
		}
	}
}
