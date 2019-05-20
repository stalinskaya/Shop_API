using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.DAL.EF
{
	public class ShopContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<LogDetail> LogDetails { get; set; }
		public DbSet<FileModel> Files { get; set; }

		public ShopContext(DbContextOptions<ShopContext> options)
			: base(options)
		{
			//Database.EnsureCreated();
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderProduct>()
							.HasKey(t => new { t.OrderID, t.ProductID });

			modelBuilder.Entity<OrderProduct>()
				.HasOne(pt => pt.Order)
				.WithMany(p => p.OrderProducts)
				.HasForeignKey(pt => pt.OrderID);

			modelBuilder.Entity<OrderProduct>()
				.HasOne(pt => pt.Product)
				.WithMany(t => t.OrderProducts)
				.HasForeignKey(pt => pt.ProductID);

			base.OnModelCreating(modelBuilder);
		}

	}
}
