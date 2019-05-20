using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
	public class OrderProduct
	{
		[Key, Column(Order = 0)]
		public int OrderID { get; set; }
		[Key, Column(Order = 1)]
		public int ProductID { get; set; }

		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }

		public int Amount { get; set; }
	}
}
