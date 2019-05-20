using Shop.BLL.Infrastructure;
using Shop.BLL.Interfaces;
using Shop.DAL.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.Services
{
	public class OrderService : IOrderService
	{
		IUnitOfWork Database { get; set; }

		public OrderService(IUnitOfWork uow)
		{
			Database = uow;
		}

		public void ProcessOrder(List<Item> cart, string user)
		{
			Order order = new Order()
			{
				OrderDate = DateTime.Now,
				ApplicationUserId = user
			};

			Database.Orders.Create(order);
			Database.Save();

			foreach (var item in cart)
			{
				OrderProduct orderProduct = new OrderProduct
				{
					ProductID = item.Product.ProductID,
					OrderID = order.OrderID,
					Amount = item.Quantity
				};
				Database.OrderProducts.Create(orderProduct);
				Database.Save();
			}
		}


		public IEnumerable<Order> GetAllOrders()
		{
			var orders = Database.Orders.GetAll();
			return orders;
		}

		public Order GetOrder(int? id)
		{
			if (id == null)
				throw new ValidationException("Invalid id", "");
			return Database.Orders.Get(id.Value);
		}

		public void Dispose()
		{
			Database.Dispose();
		}
	}
}
