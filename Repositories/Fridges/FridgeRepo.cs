using AFayedFarm.Dtos;
using AFayedFarm.Enums;
using AFayedFarm.Global;
using AFayedFarm.Model;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm.Repositories.Fridges
{
	public class FridgeRepo : IFridgeRepo
	{
		private readonly FarmContext context;

		public FridgeRepo(FarmContext context)
		{
			this.context = context;
		}

		public async Task<RequestResponse<FridgeDto>> AddFridgeAsync(AddFridgeDto dto)
		{
			var response = new RequestResponse<FridgeDto>() { ResponseID = 0, ResponseValue = new FridgeDto() };
			var fridgedb = context.Fridges.Where(f => f.FridgeName!.ToLower() == dto.Name!.ToLower()).FirstOrDefault();
			if (fridgedb == null)
			{
				var Fridge = new Fridge()
				{
					FridgeName = dto.Name,
					Created_Date = DateTime.Now
				};
				await context.Fridges.AddAsync(Fridge);
				await context.SaveChangesAsync();


				response.ResponseValue.Name = Fridge.FridgeName;
				response.ResponseValue.ID = Fridge.FridgeID;
				response.ResponseValue.Created_Date = Fridge.Created_Date ?? DateTime.Now;
				//response.ResponseValue.Created_Date = DateOnly.FromDateTime(Fridge.Created_Date ?? DateTime.Now.Date);
				response.ResponseValue.Total = Fridge.TotalRemaining != null ? Fridge.TotalRemaining : 0;
				response.ResponseID = 1;
			}
			return response;
		}

		public async Task<RequestResponse<FridgeRecordDto>> AddFridgeRecord(AddFridgeRecordDto dto)
		{
			var response = new RequestResponse<FridgeRecordDto> { ResponseID = 0,ResponseValue = new FridgeRecordDto() };
			var fridgeProduct = new FridgeRecord();
			if (dto != null)
			{
				var remaining = (dto.Total - dto.Payed);
				#region Add Transactions to Financial Safe

				var transaction = new SafeTransaction();
				transaction.SafeID = 2;
				transaction.FridgeID = dto.FridgeID;
				transaction.TypeID = dto.TypeId;
				transaction.Type = ((TransactionType)dto.TypeId!).ToString();
				transaction.Total = -1 * dto.Payed;
				transaction.Notes = dto.Notes;
				transaction.IsfromRecord = true;
				transaction.Created_Date = DateTime.Now;

				await context.SafeTransactions.AddAsync(transaction);

				var financialSafe = await context.Safe.FindAsync(2);
				if (dto.TypeId == (int)TransactionType.Pay)
					financialSafe!.Total = financialSafe.Total - dto.Payed;

				context.Safe.Update(financialSafe!);

				var fridgeDb = await context.Fridges.Where(f => f.FridgeID == dto.FridgeID).FirstOrDefaultAsync();
				if (fridgeDb!.TotalRemaining == null)
					fridgeDb.TotalRemaining = 0;
				fridgeDb!.TotalRemaining += remaining;
				context.Fridges.Update(fridgeDb);

				await context.SaveChangesAsync();
				#endregion


				fridgeProduct.FridgeID = dto.FridgeID;
				fridgeProduct.ProductID = dto.ProductID;
				fridgeProduct.Action = dto.Action;
				fridgeProduct.ActionName = ((FridgeActionEnum)dto.Action!).ToString();
				fridgeProduct.SupplyDate = dto.SupplyDate ?? DateTime.Now;
				fridgeProduct.Created_Date = DateTime.Now;
				fridgeProduct.Number = dto.Number;
				fridgeProduct.Quantity = dto.Quantity;
				fridgeProduct.Price = dto.Price;
				fridgeProduct.Total = dto.Total;
				fridgeProduct.Payed = dto.Payed;
				fridgeProduct.Remaining = remaining;
				fridgeProduct.FridgeNotes = dto.Notes;
				fridgeProduct.CarNumber = dto.CarNumber;
				fridgeProduct.FinancialId = transaction.ID;

				await context.FridgeRecords.AddAsync(fridgeProduct);
				await context.SaveChangesAsync();

				// TO DO
				// Remove From Store 
				// Add To Fridge According to Action Type Enum

				var addRemoveDto = new AddRemoveProductFridgeDto()
				{
					FridgeID = dto.FridgeID,
					ProductID = dto.ProductID,
					Quantity = dto.Quantity
				};

				if (dto.Action == (int)FridgeActionEnum.Entry)
				{
					await RemoveProductFromStore(addRemoveDto);
					await AddProductToFridge(addRemoveDto);
				}
				if (dto.Action == (int)FridgeActionEnum.Exit)
				{
					await AddProductToStore(addRemoveDto);
					await RemoveProductFromFridge(addRemoveDto);
				}



				// TO DO ==> Get FridgeRecordByID()

				var fridgerecordDto = await GetFridgeRecordByID(fridgeProduct.FridgeRecordID);

				response.ResponseID = 1;
				response.ResponseValue = fridgerecordDto.ResponseValue;
			}
			return response;

		}

		public async Task<RequestResponse<FridgeDto>> GetFridgeById(int id)
		{
			var response = new RequestResponse<FridgeDto> { ResponseID = 0, ResponseValue = new FridgeDto() };
			var fridgeDb = await context.Fridges.FindAsync(id);
			//var total = await GetTotalRemaining(id);
			if (fridgeDb != null)
			{
				var Fridge = new FridgeDto()
				{
					Name = fridgeDb.FridgeName,
					ID = fridgeDb.FridgeID,
					Created_Date = fridgeDb.Created_Date ?? DateTime.Now,
					//Created_Date = DateOnly.FromDateTime(fridgeDb.Created_Date ?? DateTime.Now.Date),
					Total = fridgeDb.TotalRemaining == null ? 0 : fridgeDb.TotalRemaining
				};
				response.ResponseID = 1;
				response.ResponseValue = Fridge;
			}
			return response;
		}

		public async Task<RequestResponse<List<FridgeDto>>> GetFridgesAsync()
		{
			var response = new RequestResponse<List<FridgeDto>> { ResponseID = 0,ResponseValue = new List<FridgeDto>() };
			var allFridges = new List<FridgeDto>();
			var fridgesDb = await context.Fridges.OrderByDescending(c => c.Created_Date).ToListAsync();
			if (fridgesDb.Count != 0)
			{
				foreach (var item in fridgesDb)
				{
					var fridge = new FridgeDto();
					fridge.ID = item.FridgeID;
					fridge.Name = item.FridgeName;
					fridge.Total = item.TotalRemaining;
					fridge.Created_Date = item.Created_Date ?? DateTime.Now;
					//fridge.Created_Date = DateOnly.FromDateTime(item.Created_Date ?? DateTime.Now);
					if (item.TotalRemaining == null)
					{
						var remaning = await CalculateTotalRemainingFromRecords(item.FridgeID);
						fridge.Total = remaning.ResponseValue;
					}
					allFridges.Add(fridge);
				}
				response.ResponseID = 1;
				response.ResponseValue = allFridges;
				return response;
			}
			response.ResponseValue = allFridges;
			return response;
		}

		public async Task<RequestResponse<FridgeDto>> UpdateFridge(int id, AddFridgeDto dto)
		{
			var response = new RequestResponse<FridgeDto> { ResponseID = 0, ResponseValue = new FridgeDto() };

			var fridgeDb = await context.Fridges.SingleOrDefaultAsync(m => m.FridgeID == id);
			if (fridgeDb != null)
			{
				fridgeDb.FridgeName = dto.Name;
				context.Fridges.Update(fridgeDb);
				await context.SaveChangesAsync();

				var fridge = await GetFridgeById(id);

				response.ResponseID = 1;
				response.ResponseValue = fridge.ResponseValue;

			}

			return response;
		}

		public async Task<RequestResponse<bool>> RemoveProductFromStore(AddRemoveProductFridgeDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };
			var productInStore = await context.StoreProducts.Where(c => c.ProductID == dto.ProductID).FirstOrDefaultAsync();
			if (productInStore != null)
			{
				if (productInStore.Quantity == null)
					productInStore.Quantity = 0;

				productInStore.Quantity -= dto.Quantity;
				context.StoreProducts.Update(productInStore);
				await context.SaveChangesAsync();
				response.ResponseID = 1;
				response.ResponseValue = true;
				return response;
			}
			else
			{
				var newProductInStore = new StoreProduct();
				newProductInStore.ProductID = dto.ProductID;
				newProductInStore.StoreID = 2;
				newProductInStore.Created_Date = DateTime.Now;
				newProductInStore.Quantity = dto.Quantity;

				await context.StoreProducts.AddAsync(newProductInStore);
				await context.SaveChangesAsync();

				response.ResponseID = 1;
				response.ResponseValue = true;

				return response;
			}
		}

		public async Task<RequestResponse<bool>> AddProductToFridge(AddRemoveProductFridgeDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };

			var productInFridge = await context.FridgeProducts.Where(c => c.ProductID == dto.ProductID && c.FridgeID == dto.FridgeID).FirstOrDefaultAsync();
			if (productInFridge != null)
			{
				if (productInFridge.Quantity == null)
					productInFridge.Quantity = 0;
				productInFridge.Quantity += dto.Quantity;
				context.FridgeProducts.Update(productInFridge);
				await context.SaveChangesAsync();
			}
			else
			{
				var fridgeProduct = new FridgeProduct();
				if (dto != null)
				{
					fridgeProduct.ProductID = dto.ProductID;
					fridgeProduct.FridgeID = dto.FridgeID;
					fridgeProduct.Created_Date = DateTime.Now;
					fridgeProduct.Quantity = dto.Quantity;

					await context.FridgeProducts.AddAsync(fridgeProduct);
					await context.SaveChangesAsync();
				}
			}

			response.ResponseID = 1;
			response.ResponseValue = true;
			return response;
		}

		public async Task<RequestResponse<bool>> RemoveProductFromFridge(AddRemoveProductFridgeDto dto)
		{
			var response = new RequestResponse<bool> { ResponseID = 0, ResponseValue = false };

			var productInFridge = await context.FridgeProducts.Where(c => c.ProductID == dto.ProductID && c.FridgeID == dto.FridgeID).FirstOrDefaultAsync();
			if (productInFridge != null)
			{
				if (productInFridge.Quantity == 0)
					return response;

				productInFridge.Quantity -= dto.Quantity;
				context.FridgeProducts.Update(productInFridge);
				await context.SaveChangesAsync();

				response.ResponseID = 1;
				response.ResponseValue = true;
			}

			return response;
		}

		public async Task<RequestResponse<FridgeRecordDto>> GetFridgeRecordByID(int recordID)
		{
			var response = new RequestResponse<FridgeRecordDto> { ResponseID = 0, ResponseValue = new FridgeRecordDto() };
			var record = new FridgeRecordDto();
			var recordDb = await context.FridgeRecords.Include(c => c.Fridge).Include(c => c.Product).Where(c => c.FridgeRecordID == recordID).FirstOrDefaultAsync();
			if (recordDb != null)
			{
				record.FridgeRecordID = recordDb.FridgeRecordID;
				record.FridgeID = recordDb.Fridge!.FridgeID;
				record.FridgeName = recordDb.Fridge.FridgeName;
				record.ProductID = recordDb?.Product?.ProductID;
				record.ProductName = recordDb?.Product?.ProductName;
				record.SupplyDate = recordDb.SupplyDate.HasValue ? recordDb.SupplyDate : DateTime.Now;
				record.Created_Date = recordDb.Created_Date.HasValue ? recordDb.Created_Date : DateTime.Now;
				record.Number = recordDb.Number;
				record.Quantity = recordDb.Quantity;
				record.Price = recordDb.Price;
				record.Total = recordDb.Total;
				record.Payed = recordDb.Payed;
				record.Remaining = recordDb.Remaining;
				record.Notes = recordDb.FridgeNotes;
				record.CarNumber = recordDb.CarNumber;
				record.Action = recordDb.Action;
				record.ActionName = ((FridgeActionEnum)recordDb.Action!).ToString();

				response.ResponseID = 1;
				response.ResponseValue = record;
			}
			return response;
		}

		public async Task<RequestResponse<List<FridgeRecordDto>>> GetAllFridgesRecords(int fridgeID)
		{
			var response = new RequestResponse<List<FridgeRecordDto>>() { ResponseID = 0 };
			var fridgeRecords = new List<FridgeRecordDto>();
			var records = await context.FridgeRecords.Include(c => c.Fridge).Include(c => c.Product).OrderByDescending(c => c.FridgeRecordID).Where(c => c.FridgeID == fridgeID).ToListAsync();
			var transactionRecordDb = await context.SafeTransactions
				.Include(c => c.Farm).
				Where(c => c.FridgeID == fridgeID)
				.OrderByDescending(c => c.Created_Date).ToListAsync();

			if (records.Count != 0 || transactionRecordDb.Count != 0)
			{
				if (records.Count != 0)
				{
					foreach (var item in records)
					{
						//remaining = item.TotalPrice - item.Paied;
						var record = new FridgeRecordDto();

						record.FridgeRecordID = item.FridgeRecordID;
						record.FridgeID = item.Fridge!.FridgeID;
						record.FridgeName = item.Fridge.FridgeName;
						record.ProductID = item?.Product?.ProductID;
						record.ProductName = item?.Product?.ProductName;
						record.SupplyDate = item.SupplyDate.HasValue ? item.SupplyDate : DateTime.Now;
						record.Created_Date = item.Created_Date.HasValue ? item.Created_Date : DateTime.Now;
						record.Quantity = item.Quantity;
						record.Price = item.Price;
						record.Payed = item.Payed;
						record.Remaining = item.Remaining;
						record.Notes = item.FridgeNotes;
						record.CarNumber = item.CarNumber;
						record.Action = item.Action;
						record.ActionName = ((FridgeActionEnum)item.Action!).ToString();

						fridgeRecords.Add(record);
					}
				}

				if (transactionRecordDb.Count != 0)
				{
					foreach (var item in transactionRecordDb)
					{
						var transactionRecord = new FridgeRecordDto();
						transactionRecord.FridgeRecordID = item.ID;
						transactionRecord.FridgeID = (int)item.Fridge!.FridgeID;
						transactionRecord.FridgeName = item.Fridge!.FridgeName;
						transactionRecord.Description = TransactionType.Pay.ToString();
						transactionRecord.Payed = -1 * item.Total;
						transactionRecord.Notes = item.Notes;
						transactionRecord.Created_Date = item.Created_Date;

						fridgeRecords.Add(transactionRecord);
					}

				}

				response.ResponseID = 1;
				response.ResponseValue = fridgeRecords;
			}
			return response;
		}

		public async Task<RequestResponse<AllFridgeRecordsWithTotalDto>> GetAllFridgeRecordsWithTotal(int fridgeID)
		{
			var response = new RequestResponse<AllFridgeRecordsWithTotalDto>() { ResponseID = 0, ResponseValue = new AllFridgeRecordsWithTotalDto() };
			var fridgeRecord = new List<FridgeRecordDto>();
			var records = await context.FridgeRecords
				.Include(c => c.Fridge)
				.Include(c => c.Product)
				.OrderByDescending(c => c.Created_Date)
				.Where(c => c.FridgeID == fridgeID).ToListAsync();
			var transactionRecordDb = await context.SafeTransactions
				.Include(c => c.Farm)
				.Where(c => c.FridgeID == fridgeID)
				.OrderByDescending(c => c.Created_Date).ToListAsync();
			if (records.Count != 0 || transactionRecordDb.Count != 0)
			{
				if (records.Count != 0)
				{
					//decimal? remaining = 0;
					foreach (var item in records)
					{
						//remaining = item.TotalPrice - item.Paied;
						var record = new FridgeRecordDto();

						record.FridgeRecordID = item.FridgeRecordID;
						record.FridgeID = item.Fridge!.FridgeID;
						record.FridgeName = item.Fridge.FridgeName;
						record.ProductID = item?.Product?.ProductID;
						record.ProductName = item?.Product?.ProductName;
						record.SupplyDate = item.SupplyDate.HasValue ? item.SupplyDate : DateTime.Now;
						record.Created_Date = item.Created_Date.HasValue ? item.Created_Date : DateTime.Now;
						record.Quantity = item.Quantity;
						record.Price = item.Price;
						record.Number = item.Number;
						record.Total = item.Total;
						record.Payed = item.Payed;
						record.Remaining = item.Remaining;
						record.Notes = item.FridgeNotes;
						record.CarNumber = item.CarNumber;
						record.Action = item.Action;
						record.ActionName = ((FridgeActionEnum)item.Action!).ToString();

						fridgeRecord.Add(record);
					}
				}
				if (transactionRecordDb.Count != 0)
				{
					foreach (var item in transactionRecordDb)
					{
						var transactionRecord = new FridgeRecordDto();
						transactionRecord.FridgeID = (int)item.Fridge!.FridgeID;
						transactionRecord.FridgeName = item.Fridge!.FridgeName;
						transactionRecord.Description = TransactionType.Pay.ToString();
						transactionRecord.Payed = -1 * item.Total;
						transactionRecord.Notes = item.Notes;

						fridgeRecord.Add(transactionRecord);
					}
				}

				//var TotalRemaining = await GetTotalRemaining(farmID);
				var fridgeDb = await context.Fridges.Where(f => f.FridgeID == fridgeID).FirstOrDefaultAsync();
				var TotalRemaining = fridgeDb!.TotalRemaining;

				response.ResponseValue.TotalRemaining = TotalRemaining;
				response.ResponseValue.FridgeRecords = fridgeRecord;
				response.ResponseID = 1;
				return response;
			}

			return response;
		}

		public async Task<RequestResponse<FridgeRecordsWithDataDto>> GetFridgeRecordWithFridgeDataByID(int fridgeID, int currentPage, int pageSize)
		{
			var response = new RequestResponse<FridgeRecordsWithDataDto>() { ResponseID = 0, ResponseValue = new FridgeRecordsWithDataDto() };
			var fridgeRecord = new List<FridgeRecordDto>();

			var fridgerecords = await context.FridgeRecords
				.Include(c => c.Fridge)
				.Include(c => c.Product)
				.OrderByDescending(c => c.Created_Date)
				.Where(c => c.FridgeID == fridgeID).ToListAsync();

			var records = fridgerecords.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			var transactionRecordDbs = await context.SafeTransactions
				.Include(c => c.Farm)
				.OrderByDescending(c => c.Created_Date)
				.Where(c => c.FridgeID == fridgeID && c.IsfromRecord == false).ToListAsync();

			var transactionRecordDb = transactionRecordDbs.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			if (records.Count != 0 || transactionRecordDb.Count != 0)
			{
				var index = 1;
				if (records.Count != 0)
				{
					foreach (var item in records)
					{
						var record = new FridgeRecordDto();


						record.Id = index;
						record.FridgeRecordID = item.FridgeRecordID;
						record.FridgeID = item.Fridge!.FridgeID;
						record.FridgeName = item.Fridge.FridgeName;
						record.ProductID = item?.Product?.ProductID;
						record.ProductName = item?.Product?.ProductName;
						record.SupplyDate = item!.SupplyDate;
						record.Created_Date = item.Created_Date.HasValue ? item.Created_Date : DateTime.Now;
						record.Number = item.Number;
						record.Quantity = item.Quantity;
						record.Total = item.Total;
						record.Price = item.Price;
						record.Payed = item.Payed;
						record.Remaining = item.Remaining;
						record.Notes = item.FridgeNotes;
						record.CarNumber = item.CarNumber;
						record.Action = item.Action;
						record.ActionName = ((FridgeActionEnum)item.Action!).ToString();
						++index;

						fridgeRecord.Add(record);
					}
				}
				if (transactionRecordDb.Count != 0)
				{
					foreach (var item in transactionRecordDb)
					{
						var transactionRecord = new FridgeRecordDto();

						transactionRecord.Id = index;
						transactionRecord.FridgeRecordID = item.ID;
						transactionRecord.Created_Date = item.Created_Date;
						transactionRecord.FridgeID = (int)item.Fridge!.FridgeID;
						transactionRecord.FridgeName = item.Fridge!.FridgeName;
						transactionRecord.Description = TransactionType.Pay.ToString();
						transactionRecord.Payed = -1 * item.Total;
						transactionRecord.Notes = item.Notes;
						++index;

						fridgeRecord.Add(transactionRecord);
					}
				}

				var fridgeDataWithRecord = await GetFridgeById(fridgeID);
				var fridgeDB = await context.Fridges.Where(f => f.FridgeID == fridgeID).FirstOrDefaultAsync();
				var TotalRemainingWithRecord = fridgeDB!.TotalRemaining;

				var totalRecords = fridgerecords.Count() + transactionRecordDbs.Count();

				response.LastPage = (int)Math.Ceiling((double)totalRecords / pageSize);
				response.CurrentPage = currentPage;
				response.PageSize = pageSize;
				response.TotalRecords = totalRecords;

				response.ResponseValue.Name = fridgeDataWithRecord.ResponseValue!.Name;
				response.ResponseValue.ID = fridgeDataWithRecord.ResponseValue.ID;
				response.ResponseValue.Total = TotalRemainingWithRecord;
				response.ResponseValue.Date = fridgeDataWithRecord.ResponseValue.Created_Date;
				response.ResponseValue.FridgeRecords = fridgeRecord;

				response.ResponseID = 1;
				return response;
			}
			var fridgeData = await GetFridgeById(fridgeID);
			var TotalRemaining = await CalculateTotalRemainingFromRecords(fridgeID);

			response.ResponseValue.Name = fridgeData.ResponseValue!.Name;
			response.ResponseValue.ID = fridgeData.ResponseValue!.ID;
			response.ResponseValue.Date = fridgeData.ResponseValue!.Created_Date;
			response.ResponseValue.Total = TotalRemaining.ResponseValue;
			response.ResponseValue.FridgeRecords = fridgeRecord;
			return response;
		}

		public async Task<RequestResponse<decimal>> CalculateTotalRemainingFromRecords(int fridgeID)
		{
			var response = new RequestResponse<decimal> { ResponseID = 0, ResponseValue = 0 };
			var fridgeList = await GetAllFridgesRecords(fridgeID);
			if (fridgeList.ResponseID == 1)
			{
				decimal? total = 0;
				foreach (var item in fridgeList.ResponseValue!)
				{
					total += item.Remaining;
				}
				response.ResponseID = 1;
				response.ResponseValue = (decimal)total!;

				var fridgeDb = await context.Fridges.Where(c => c.FridgeID == fridgeID).FirstOrDefaultAsync();
				fridgeDb!.TotalRemaining = total;
				context.Fridges.Update(fridgeDb);
				await context.SaveChangesAsync();
			}
			return response;
		}

		public async Task<RequestResponse<FridgeRecordDto>> UpdateFridgeRecordAsync(int recordID, AddFridgeRecordDto dto)
		{
			var response = new RequestResponse<FridgeRecordDto> { ResponseID = 0,ResponseValue = new FridgeRecordDto() };
			var fridgeProduct = await context.FridgeRecords
				.Include(c => c.Fridge)
				.Include(c => c.Product)
				.Where(c => c.FridgeRecordID == recordID).FirstOrDefaultAsync();

			var fridgeDb = await context.Fridges.Where(f => f.FridgeID == fridgeProduct.FridgeID).FirstOrDefaultAsync();


			if (fridgeProduct != null)
			{

				var incomeQuantity = dto.Quantity;

				// TO DO
				// Remove From Store 
				// Add To Fridge According to Action Type Enum

				var addRemoveDto = new AddRemoveProductFridgeDto();

				addRemoveDto.FridgeID = dto.FridgeID;
				addRemoveDto.ProductID = dto.ProductID;

				if (dto.Action == (int)FridgeActionEnum.Entry && dto.Quantity > fridgeProduct.Quantity)
				{
					//dto.Quantity = dto.Quantity - fridgeProduct.Quantity;

					addRemoveDto.Quantity = dto.Quantity - fridgeProduct.Quantity;

					await RemoveProductFromStore(addRemoveDto);
					await AddProductToFridge(addRemoveDto);
				}
				else if (dto.Action == (int)FridgeActionEnum.Entry && dto.Quantity < fridgeProduct.Quantity)
				{

					addRemoveDto.Quantity = fridgeProduct.Quantity - dto.Quantity;
					await RemoveProductFromFridge(addRemoveDto);
					await AddProductToStore(addRemoveDto);
				}

				if (dto.Action == (int)FridgeActionEnum.Exit && dto.Quantity > fridgeProduct.Quantity)
				{
					addRemoveDto.Quantity = dto.Quantity - fridgeProduct.Quantity;
					await RemoveProductFromFridge(addRemoveDto);
				}
				else if (dto.Action == (int)FridgeActionEnum.Exit && fridgeProduct.Quantity > dto.Quantity)
				{
					addRemoveDto.Quantity = fridgeProduct.Quantity - dto.Quantity;
					await AddProductToFridge(addRemoveDto);
				}

				if (dto.TypeId == (int)TransactionType.Pay && dto.Payed != fridgeProduct.Payed)
				{

					decimal? oldPayAmount = 0;
					decimal? incomePayed = dto.Payed;

					var financailReocrdDb = await context.SafeTransactions.Where(p => p.ID == fridgeProduct.FinancialId).FirstOrDefaultAsync();
					#region Remove Financial Record from database
					if (financailReocrdDb != null)
					{
						oldPayAmount = -1 * financailReocrdDb.Total;

						financailReocrdDb.SafeID = 2;
						financailReocrdDb.FridgeID = dto.FridgeID;
						financailReocrdDb.TypeID = dto.TypeId;
						financailReocrdDb.Type = dto.TypeId != null ? ((TransactionType)dto.TypeId).ToString() : TransactionType.Pay.ToString();
						financailReocrdDb.Total = -1 * dto.Payed;
						financailReocrdDb.Notes = dto.Notes;
						financailReocrdDb.Created_Date = DateTime.Now;
						financailReocrdDb.IsfromRecord = true;

						context.SafeTransactions.Update(financailReocrdDb);
						await context.SaveChangesAsync();

						var safe = await context.Safe.FindAsync(2);
						safe!.Total += oldPayAmount;
						if (dto.TypeId == (int)TransactionType.Pay)
							safe!.Total = safe.Total - dto.Payed;

						context.Safe.Update(safe);

						await context.SaveChangesAsync();

					}
					#endregion

					if (fridgeDb != null)
					{
						if (fridgeDb.TotalRemaining == null)
							fridgeDb.TotalRemaining = 0;

						if (dto.TypeId == (int)TransactionType.Pay && fridgeProduct.Payed != incomePayed)
							fridgeDb!.TotalRemaining = fridgeDb.TotalRemaining - (incomePayed - fridgeProduct.Payed);

						context.Fridges.Update(fridgeDb!);

						await context.SaveChangesAsync();
					}
				}

				var oldRemaining = fridgeProduct.Remaining;
				var newRemaining = dto.Total - dto.Payed;

				if(newRemaining != oldRemaining && dto.Payed == fridgeProduct.Payed)
				{
					if (fridgeDb != null)
					{
						if (fridgeDb.TotalRemaining == null)
							fridgeDb.TotalRemaining = 0;

						fridgeDb.TotalRemaining = fridgeDb.TotalRemaining + (newRemaining - oldRemaining);

						context.Fridges.Update(fridgeDb!);
						await context.SaveChangesAsync();
					}
				}


				fridgeProduct.FridgeID = dto.FridgeID;
				fridgeProduct.ProductID = dto.ProductID;
				fridgeProduct.Action = dto.Action;
				fridgeProduct.ActionName = ((FridgeActionEnum)dto.Action!).ToString();
				fridgeProduct.SupplyDate = dto.SupplyDate ?? DateTime.Now;
				fridgeProduct.Number = dto.Number;
				fridgeProduct.Quantity = incomeQuantity;
				fridgeProduct.Price = dto.Price;
				fridgeProduct.Total = dto.Total;
				fridgeProduct.Payed = dto.Payed;
				fridgeProduct.Remaining = dto.Total - dto.Payed;
				fridgeProduct.FridgeNotes = dto.Notes;
				fridgeProduct.CarNumber = dto.CarNumber;
				fridgeProduct.FinancialId = fridgeProduct.FinancialId;

				context.FridgeRecords.Update(fridgeProduct);
				await context.SaveChangesAsync();



				// TO DO ==> Get FridgeRecordByID()

				var farmrecordDto = await GetFridgeRecordByID(fridgeProduct.FridgeRecordID);

				response.ResponseID = 1;
				response.ResponseValue = farmrecordDto.ResponseValue;
			}
			return response;
		}

		public async Task<RequestResponse<FridgeDto>> PayToFridge(FridgePaymentDto dto)
		{
			var response = new RequestResponse<FridgeDto> { ResponseID = 0, ResponseValue = new FridgeDto() };
			var fridgeDb = await context.Fridges.Where(e => e.FridgeID == dto.Id).FirstOrDefaultAsync();
			if (fridgeDb == null)
				return response;

			#region Add Transactions to Financial Safe
			var transaction = new SafeTransaction()
			{
				SafeID = 2,
				FridgeID = dto.Id,
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

			// Subtract Pay Amount from Total Remaining of Fridge
			fridgeDb.TotalRemaining -= dto.Total;
			context.Fridges.Update(fridgeDb);

			await context.SaveChangesAsync();
			#endregion

			var fridge = await GetFridgeById((int)dto.Id);

			response.ResponseID = 1;
			response.ResponseValue = fridge.ResponseValue;
			return response;
		}

		public async Task<RequestResponse<bool>> AddProductToStore(AddRemoveProductFridgeDto dto)
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
			}
			else
			{
				var storeProduct = new StoreProduct();
				if (dto != null)
				{
					storeProduct.ProductID = dto.ProductID;
					storeProduct.StoreID = 2;
					storeProduct.Created_Date = DateTime.Now;
					storeProduct.Quantity = dto.Quantity;

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
