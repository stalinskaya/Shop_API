using Shop.BLL.Interfaces;
using Shop.DAL.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.BLL.Services
{
	public class OrderProductService : IOrderProductService
	{
		IUnitOfWork Database { get; set; }

		public OrderProductService(IUnitOfWork uow)
		{
			Database = uow;
		}
		public IEnumerable<OrderProduct> GetAll()
		{
			return Database.OrderProducts.GetAll();
		}
		public void Dispose()
		{
			Database.Dispose();
		}
	}
}
