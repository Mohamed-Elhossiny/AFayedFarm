﻿using AFayedFarm.Dtos;
using AFayedFarm.Global;

namespace AFayedFarm.Repositories.Store
{
	public interface IStoreRepo
	{
		Task<RequestResponse<List<StoreProductDto>>> GetStoreProducts(int currentPage, int pageSize);
		Task<RequestResponse<StoreProductDto>> SetProductQtyToZero(int productID);

	}
}
