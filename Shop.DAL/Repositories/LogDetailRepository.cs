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
	public class LogDetailRepository : IRepository<LogDetail>
	{
		private ShopContext db;

		public LogDetailRepository(ShopContext context)
		{
			this.db = context;
		}

		public IEnumerable<LogDetail> GetAll()
		{
			return db.LogDetails.ToList();
		}

		public LogDetail Get(int id)
		{
			return db.LogDetails.Find(id);
		}

		public void Create(LogDetail logDetail)
		{
			db.LogDetails.Add(logDetail);
		}

		public void Update(LogDetail logDetail)
		{
			db.Entry(logDetail).State = EntityState.Modified;
		}
		public IEnumerable<LogDetail> Find(Func<LogDetail, Boolean> predicate)
		{
			return db.LogDetails.Where(predicate).ToList();
		}
		public void Delete(int id)
		{
			LogDetail logDetail = db.LogDetails.Find(id);
			if (logDetail != null)
				db.LogDetails.Remove(logDetail);
		}
	}
}
