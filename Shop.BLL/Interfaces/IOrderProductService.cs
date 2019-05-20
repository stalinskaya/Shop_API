using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.BLL.Interfaces
{
	public interface IOrderProductService
	{
		IEnumerable<OrderProduct> GetAll();
		void Dispose();
	}
}
