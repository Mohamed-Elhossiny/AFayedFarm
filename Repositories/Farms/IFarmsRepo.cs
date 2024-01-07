using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Supplier
{
	public interface IFarmsRepo
	{
		Task<FarmDto> AddFarmAsync(AddFarmDto farmDto);
		Task<FarmDto> GetFarmByName(string farmName);
		Task<List<FarmDto>> GetFarmsAsync();
		Task<FarmDto> GetFarmById(int id);
		Task<RequestResponse<FarmDto>> UpdateFarm(int id,AddFarmDto farmDto);
		Task<RequestResponse<FarmRecordDto>> AddFarmRecord(AddFarmRecordDto farmDto);
		Task<RequestResponse<List<FarmRecordDto>>> GetAllFarmRecords(int farmID);
		Task<RequestResponse<List<FarmRecordWithoutDescriptionDto>>> GetProductsDetails(/*int productId*/);

		Task<RequestResponse<FarmRecordsWithTotalRemainingDto>> GetAllFarmRecordsWithTotal(int farmID);
		Task<RequestResponse<FarmRecordDto>> GetFarmRecordByID(int recordID);
		Task<RequestResponse<FarmRecordsWithFarmDataDto>> GetFarmRecordWithFarmDataByID(int farmID);
		Task<RequestResponse<FarmRecordDto>> UpdateFarmRecordAsync(int recordID,AddFarmRecordDto dto);
		Task<RequestResponse<decimal>> CalculateTotalRemainingFromRecords(int farmsID);
		Task<RequestResponse<bool>> AddProductToStore(AddFarmRecordDto dto);
		Task<RequestResponse<bool>> PayToFarm(FarmPaymentDto dto);

	}
}
