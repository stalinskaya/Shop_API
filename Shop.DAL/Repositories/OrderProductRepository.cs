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
	public class OrderProductRepository : IRepository<OrderProduct>
	{
		private ShopContext db;

		public OrderProductRepository(ShopContext context)
		{
			this.db = context;
		}

		public IEnumerable<OrderProduct> GetAll()
		{
			return db.OrderProducts.Include(o => o.Product).Include(n => n.Order);
		}

		public OrderProduct Get(int id)
		{
			return db.OrderProducts.Find(id);
		}

		public void Create(OrderProduct orderproduct)
		{
			db.OrderProducts.Add(orderproduct);
		}

		public void Update(OrderProduct orderproduct)
		{
			db.Entry(orderproduct).State = EntityState.Modified;
		}
		public IEnumerable<OrderProduct> Find(Func<OrderProduct, Boolean> predicate)
		{
			return db.OrderProducts.Include(o => o.Product).Include(n => n.Order).Where(predicate).ToList();
		}
		public void Delete(int id)
		{
			OrderProduct orderproduct = db.OrderProducts.Find(id);
			if (orderproduct != null)
				db.OrderProducts.Remove(orderproduct);
		}
	}
}