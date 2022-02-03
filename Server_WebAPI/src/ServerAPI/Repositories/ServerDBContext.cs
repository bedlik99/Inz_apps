using Microsoft.EntityFrameworkCore;
using ServerAPI.Entities;
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
		public DbSet<Employee> EmployeeItems { get; set; }
		public DbSet<Role> RoleItems { get; set; }
		public DbSet<Laboratory> LaboratoryItems { get; set; }
		public DbSet<LaboratoryRequirement> LaboratoryRequirementsItems { get; set; }
		public DbSet<ExecutedCommand> ExecutedCommandsItems { get; set; }

		public ServerDBContext(DbContextOptions<ServerDBContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Laboratory>()
				.Property(i => i.LabName)
				.IsRequired()
				.HasMaxLength(35);

			modelBuilder.Entity<Laboratory>()
				.Property(i => i.LabOrganizer)
				.IsRequired()
				.HasMaxLength(50);

			modelBuilder.Entity<Employee>()
				.Property(i => i.Username)
				.IsRequired()
				.HasMaxLength(25);
			
			modelBuilder.Entity<Employee>()
				.Property(i => i.Password)
				.IsRequired()
				.HasMaxLength(25);

			modelBuilder.Entity<RegisteredUser>()
				.Property(i => i.Email)
				.IsRequired()
				.HasMaxLength(25);

			modelBuilder.Entity<RegisteredUser>()
				.Property(i => i.UniqueCode)
				.IsRequired()
				.HasMaxLength(8);

			modelBuilder.Entity<Role>()
				.Property(i => i.RoleName)
				.HasMaxLength(25);

			modelBuilder.Entity<ExecutedCommand>()
				.Property(i => i.Content)
				.HasMaxLength(255);

			modelBuilder.Entity<LaboratoryRequirement>()
				.Property(i => i.Content)
				.HasMaxLength(255);

			modelBuilder.Entity<RecordedEvent>()
				.Property(i => i.RegistryContent)
				.HasMaxLength(255);
		}
	}
}
