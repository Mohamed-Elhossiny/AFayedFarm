using AFayedFarm.Dtos;
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

		public async Task<FarmDto> UpdateFarm(int id,AddFarmDto farmDto)
		{
			var farmDb = await context.Farms.SingleOrDefaultAsync(m => m.FarmsID == id);
			farmDb.FarmsName = farmDto.FarmName;
			await context.SaveChangesAsync();

			return new FarmDto { FarmID = farmDb.FarmsID, FarmName = farmDb.FarmsName };
		}
	}
}
