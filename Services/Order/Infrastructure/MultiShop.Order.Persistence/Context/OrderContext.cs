﻿using Microsoft.EntityFrameworkCore;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Persistence.Context
{
	public class OrderContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=TUNAHAN\\SQLEXPRESS;Database=MultiShopOrder>Db;Integrated Security=true;TrustServerCertificate=true");
		}
		public DbSet<Address> Addresses { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Ordering> Orderings { get; set; }
	}
}