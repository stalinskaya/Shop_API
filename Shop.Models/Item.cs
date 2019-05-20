using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Shop.Models
{
	public class Item {
		public int Quantity { get; set; }
		public Product Product { get; set; }
	}
}
