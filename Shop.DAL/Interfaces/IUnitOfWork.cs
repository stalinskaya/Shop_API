using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<ApplicationUser> Users { get; }
		IRepository<Product> Products { get; }
		IRepository<Order> Orders { get; }
		IRepository<OrderProduct> OrderProducts { get; }
		IRepository<LogDetail> LogDetails { get; }
		IRepository<FileModel> Files { get; }

		Task SaveAsync();
		void Save();
	}
}
