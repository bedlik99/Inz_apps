using StudentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Data
{
	public class SqlStudentAPIRepo : IStudentAPIRepo
	{
		private readonly StudentContext _studentContext;

		public SqlStudentAPIRepo(StudentContext studentContext)
		{
			_studentContext = studentContext;
		}
		public void AddStudent(Student stud)
		{
			if (stud == null)
			{
				throw new ArgumentNullException(nameof(stud));
			}
			_studentContext.StudentItems.Add(stud);
		}

		public void DeleteStudent(Student stud)
		{
			if (stud == null)
			{
				throw new ArgumentNullException(nameof(stud));
			}
			_studentContext.StudentItems.Remove(stud);
		}

		public IEnumerable<Student> GetAllStudents()
		{
			return _studentContext.StudentItems.ToList();
		}

		public Student GetStudent(int id)
		{
			return _studentContext.StudentItems.FirstOrDefault(p => p.Id == id);
		}

		public bool SaveChanges()
		{
			return (_studentContext.SaveChanges() >= 0);
		}

		public void UpdateStudent(Student stud)
		{
			if (stud == null)
			{
				throw new ArgumentNullException(nameof(stud));
			}			
		}
	}
}
