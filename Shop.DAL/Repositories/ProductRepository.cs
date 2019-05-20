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
	public class ProductRepository : IRepository<Product>
	{
		private ShopContext db;

		public ProductRepository(ShopContext context)
		{
			this.db = context;
		}

		public IEnumerable<Product> GetAll()
		{
			return db.Products;
		}

		public Product Get(int id)
		{
			return db.Products.Find(id);
		}

		public void Create(Product product)
		{
			db.Products.Add(product);
		}

		public void Update(Product product)
		{
			db.Entry(product).State = EntityState.Modified;
		}

		public IEnumerable<Product> Find(Func<Product, Boolean> predicate)
		{
			return db.Products.Where(predicate).ToList();
		}

		public void Delete(int id)
		{
			Product product = db.Products.Find(id);
			if (product != null)
				db.Products.Remove(product);
		}
	}
}
