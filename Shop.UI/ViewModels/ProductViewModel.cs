using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.ViewModels
{
	public class ProductViewModel
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		[DataType(DataType.Date)]
		public DateTime? AddDate { get; set; }
		public IFormFileCollection Files { get; set; }
	}
}