using AFayedFarm.Dtos;

namespace AFayedFarm.Repositories.Supplier
{
	public interface IFarmsRepo
	{
		Task<FarmDto> AddFarmAsync(AddFarmDto farmDto);
		Task<FarmDto> GetFarmByName(string farmName);
		Task<List<FarmDto>> GetFarmsAsync();
		Task<FarmDto> GetFarmById(int id);
		Task<FarmDto> UpdateFarm(int id,AddFarmDto farmDto);

	}
}
