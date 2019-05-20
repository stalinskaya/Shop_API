using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.BLL.Interfaces
{
	public interface IProductService
	{
		void MakeProduct(Product product);
		void EditProduct(Product product);
		void DeleteProduct(int id);
		Product GetProduct(int? id);
		IEnumerable<Product> GetProducts();
		void Dispose();
	}
}
