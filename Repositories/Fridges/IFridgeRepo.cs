using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Fridges
{
	public interface IFridgeRepo
	{
		Task<RequestResponse<FridgeDto>> AddFridgeAsync(AddFridgeDto dto);
		Task<RequestResponse<List<FridgeDto>>> GetFridgesAsync();
		Task<RequestResponse<FridgeDto>> GetFridgeById(int id);
		Task<RequestResponse<FridgeDto>> UpdateFridge(int id, AddFridgeDto dto);
		Task<RequestResponse<FridgeRecordDto>> AddFridgeRecord(AddFridgeRecordDto farmDto);
		Task<RequestResponse<bool>> RemoveProductFromStore(AddRemoveProductFridgeDto dto);
		Task<RequestResponse<bool>> RemoveProductFromFridge(AddRemoveProductFridgeDto dto);
		Task<RequestResponse<bool>> AddProductToFridge(AddRemoveProductFridgeDto dto);
		Task<RequestResponse<bool>> AddProductToStore(AddRemoveProductFridgeDto dto);
		Task<RequestResponse<FridgeRecordDto>> GetFridgeRecordByID(int recordID);
		Task<RequestResponse<List<FridgeRecordDto>>> GetAllFridgesRecords(int fridgeID);
		Task<RequestResponse<AllFridgeRecordsWithTotalDto>> GetAllFridgeRecordsWithTotal(int fridgeID);
		Task<RequestResponse<FridgeRecordsWithDataDto>> GetFridgeRecordWithFridgeDataByID(int farmID, int pageNumber, int pageSize);
		Task<RequestResponse<decimal>> CalculateTotalRemainingFromRecords(int farmsID);
		Task<RequestResponse<FridgeRecordDto>> UpdateFridgeRecordAsync(int recordID, AddFridgeRecordDto dto);
		Task<RequestResponse<FridgeDto>> PayToFridge(FridgePaymentDto dto);
	}
}
