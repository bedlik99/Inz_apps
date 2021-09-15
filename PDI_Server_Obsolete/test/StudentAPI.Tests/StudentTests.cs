using System;
using Xunit;
using StudentAPI.Models;

namespace StudentAPI.Tests
{
	public class StudentTests : IDisposable
	{
		Student testStudent;
		
		public StudentTests()
		{
			testStudent = new Student
			{
				IndexNum = "111111",
				IsCheater = false,
				AccessCode = "asda",
				Points = 20,
				Mark = 5,
				StartDate = "2020-01-20",
				FinishDate = "2020-01-20"
			};
		}

		public void Dispose()
		{
			testStudent = null;
		}

		[Fact]
		public void CanChangePoints()
		{
			//Arrange
		
			//Act
			testStudent.Points = 10;

			//Assert (Expectation, Actual)
			Assert.Equal(10, testStudent.Points);
		}
		[Fact]
		public void CanChangeMark()
		{
			//Arrange

			//Act
			testStudent.Mark = 2;

			//Assert (Expectation, Actual)
			Assert.Equal(2, testStudent.Mark);
		}


	}
}
