using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
	public class Order
	{
		[Key]
		public int OrderID { get; set; }

		[DataType(DataType.Date)]
		public DateTime OrderDate { get; set; }

		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }

		public ICollection<OrderProduct> OrderProducts { get; set; }
	}
}
