using Shop.BLL.Infrastructure;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.Interfaces
{
	public interface IOrderService
	{
		void ProcessOrder(List<Item> cart, string user);
		IEnumerable<Order> GetAllOrders();
		Order GetOrder(int? id);
		void Dispose();
	}
}
