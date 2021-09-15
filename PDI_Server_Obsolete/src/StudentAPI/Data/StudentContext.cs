using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Data
{
	public class StudentContext : DbContext
	{
		public StudentContext(DbContextOptions<StudentContext> options) : base(options)
		{

		}
		public DbSet<Student> StudentItems { get; set; }
	}
}
