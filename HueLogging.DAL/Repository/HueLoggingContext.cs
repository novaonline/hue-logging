﻿using HueLogging.Models;
using Microsoft.EntityFrameworkCore;

namespace HueLogging.DAL.Repository
{
	public class HueLoggingContext : DbContext
	{

		public HueLoggingContext(DbContextOptions<HueLoggingContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<LightEvent>().HasIndex(x => x.AddDate);
			modelBuilder.Entity<HueConfigStates>().HasIndex(x => x.AddDate);
		}

		public DbSet<LightEvent> LightEvent { get; set; }
		public DbSet<HueConfigStates> HueConfigStates { get; set; }
	}
}
