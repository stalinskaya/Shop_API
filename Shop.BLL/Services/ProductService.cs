using Shop.BLL.Interfaces;
using Shop.DAL.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shop.BLL.Services
{
	public class ProductService : IProductService
	{
		IUnitOfWork Database { get; set; }

		public ProductService(IUnitOfWork uow)
		{
			Database = uow;
		}
		public void MakeProduct(Product product)
		{
			Database.Products.Create(product);
			Database.Save();
		}

		public void EditProduct(Product product)
		{
			Database.Products.Update(product);
			Database.Save();
		}

		public void DeleteProduct(int id)
		{
			Database.Products.Delete(id);
			Database.Save();
		}

		public IEnumerable<Product> GetProducts()
		{
			return Database.Products.GetAll();
		}

		public Product GetProduct(int? id)
		{
			if (id == null)
				throw new ValidationException("Invalid id");
			return Database.Products.Get(id.Value);
		}

		public void Dispose()
		{
			Database.Dispose();
		}
	}
}