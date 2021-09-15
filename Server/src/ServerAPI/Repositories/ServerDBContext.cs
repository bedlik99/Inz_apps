using Microsoft.EntityFrameworkCore;
using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Repositories
{
	public class ServerDBContext : DbContext
	{
		public DbSet<RecordedEvent> RecordedEventItems { get; set; }
		public DbSet<RegisteredUser> RegisteredUserItems { get; set; }

		public ServerDBContext(DbContextOptions<ServerDBContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RegisteredUser>()
				.Property(i => i.IndexNum)
				.IsRequired()
				.HasMaxLength(6);

			modelBuilder.Entity<RegisteredUser>()
				.Property(i => i.UniqueCode)
				.IsRequired();
		}
	}
}
