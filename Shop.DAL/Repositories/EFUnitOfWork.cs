using Shop.DAL.EF;
using Shop.DAL.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Repositories
{
	public class EFUnitOfWork : IUnitOfWork
	{
		private ShopContext db;

		private ProductRepository productRepository;
		private OrderRepository orderRepository;
		private OrderProductRepository orderProductRepository;
		private ApplicationUserRepository applicationUserRepository;
		private LogDetailRepository logDetailRepository;
		private FileRepository fileRepository;

		public EFUnitOfWork(ShopContext db)
		{
			this.db = db;
		}

		public IRepository<Product> Products
		{
			get
			{
				if (productRepository == null)
					productRepository = new ProductRepository(db);
				return productRepository;
			}
		}

		public IRepository<Order> Orders
		{
			get
			{
				if (orderRepository == null)
					orderRepository = new OrderRepository(db);
				return orderRepository;
			}
		}

		public IRepository<FileModel> Files
		{
			get
			{
				if (fileRepository == null)
					fileRepository = new FileRepository(db);
				return fileRepository;
			}
		}

		public IRepository<OrderProduct> OrderProducts
		{
			get
			{
				if (orderProductRepository == null)
					orderProductRepository = new OrderProductRepository(db);
				return orderProductRepository;
			}
		}

		public IRepository<ApplicationUser> Users
		{
			get
			{
				if (applicationUserRepository == null)
					applicationUserRepository = new ApplicationUserRepository(db);
				return applicationUserRepository;
			}
		}

		public IRepository<LogDetail> LogDetails
		{
			get
			{
				if (logDetailRepository == null)
					logDetailRepository = new LogDetailRepository(db);
				return logDetailRepository;
			}
		}

		public void Save()
		{
			db.SaveChanges();
		}

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					db.Dispose();
				}
				this.disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public Task SaveAsync()
		{
			throw new NotImplementedException();
		}
	}
}

