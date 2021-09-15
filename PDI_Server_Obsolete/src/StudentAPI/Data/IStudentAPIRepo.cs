using StudentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Data
{
	public interface IStudentAPIRepo
	{
		bool SaveChanges();
		IEnumerable<Student> GetAllStudents();
		Student GetStudent(int id);
		void AddStudent(Student stud);
		void UpdateStudent(Student stud);
		void DeleteStudent(Student stud);
	}
}
