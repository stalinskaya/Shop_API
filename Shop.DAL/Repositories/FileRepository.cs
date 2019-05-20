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
	public class FileRepository : IRepository<FileModel>
	{
		private ShopContext db;

		public FileRepository(ShopContext context)
		{
			this.db = context;
		}

		public IEnumerable<FileModel> GetAll()
		{
			return db.Files;
		}

		public FileModel Get(int id)
		{
			return db.Files.Find(id);
		}

		public void Create(FileModel file)
		{
			db.Files.Add(file);
		}

		public void Update(FileModel file)
		{
			db.Entry(file).State = EntityState.Modified;
		}
		public IEnumerable<FileModel> Find(Func<FileModel, Boolean> predicate)
		{
			return db.Files.Where(predicate).ToList();
		}
		public void Delete(int id)
		{
			FileModel file = db.Files.Find(id);
			if (file != null)
				db.Files.Remove(file);
		}
	}
}
