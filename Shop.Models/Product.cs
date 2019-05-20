using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
	public class Product
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }

		[DataType(DataType.Date)]
		public DateTime AddDate { get; set; }
		
		public ICollection<OrderProduct> OrderProducts { get; set; }
		public ICollection<FileModel> Files { get; set; }
	}
}
