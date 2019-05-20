using Microsoft.EntityFrameworkCore;
using Shop.DAL.EF;
using Shop.DAL.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.DAL.Repositories
{
	public class OrderRepository : IRepository<Order>
	{
		private ShopContext db;

		public OrderRepository(ShopContext context)
		{
			this.db = context;
		}

		public IEnumerable<Order> GetAll()
		{
			return db.Orders;
		}

		public Order Get(int id)
		{
			return db.Orders.Find(id);
		}

		public void Create(Order order)
		{
			db.Orders.Add(order);
		}

		public void Update(Order order)
		{
			db.Entry(order).State = EntityState.Modified;
		}

		public IEnumerable<Order> Find(Func<Order, Boolean> predicate)
		{
			return db.Orders.Where(predicate).ToList();
		}

		public void Delete(int id)
		{
			Order order = db.Orders.Find(id);
			if (order != null)
				db.Orders.Remove(order);
		}
	}
}
