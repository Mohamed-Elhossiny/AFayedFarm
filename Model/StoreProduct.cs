﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFayedFarm.Model
{
	public class StoreProduct
	{
		[Key]
        public int StoreProductID { get; set; }

		[ForeignKey("Farms")]
		public int? FarmsID { get; set; }

		[ForeignKey("Product")]
		public int? ProductID { get; set; }

		[ForeignKey("Store")]
        public int? StoreID { get; set; }
        public virtual Farms? Farms { get; set; }
		public virtual Product? Product { get; set; }
		public virtual Store? Store { get; set; }

		[Column(TypeName = "DateTime")]
		public DateTime? SupplyDate { get; set; }

		[Column(TypeName = "DateTime")]
		public DateTime? Created_Date { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? TotalPrice { get; set; }
		public string? Notes { get; set; }
	}
}
