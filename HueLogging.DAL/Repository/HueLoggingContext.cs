using System;
using HueLogging.Models;
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
			modelBuilder.Entity<HueSession>().HasIndex(x => x.StartDate);
			modelBuilder.Entity<HueSession>().HasIndex(x => x.LightId);

		}

		public DbSet<LightEvent> LightEvent { get; set; }
		public DbSet<HueConfigStates> HueConfigStates { get; set; }
		public DbSet<HueSession> HueSessions { get; set; }
	}
}
