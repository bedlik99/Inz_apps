using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAPI.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using StudentAPI.Data;
using StudentAPI.Profiles;
using StudentAPI.Models;
using StudentAPI.DTOs;

namespace StudentAPI.Tests
{
	/// <summary>
	/// Class prepared for Unit Tests. Egxamples below
	/// </summary>

	public class StudentsControllerTests : IDisposable
	{

		Mock<IStudentAPIRepo> mockRepo;
		StudentsProfile realProfile;
		MapperConfiguration configuration;
		IMapper mapper;

		public StudentsControllerTests()
		{
			mockRepo = new Mock<IStudentAPIRepo>();
			realProfile = new StudentsProfile();
			configuration = new MapperConfiguration(cfg => cfg.
				AddProfile(realProfile));
			mapper = new Mapper(configuration);
		}
		public void Dispose()
		{
			mockRepo = null;
			mapper = null;
			configuration = null;
			realProfile = null;
		}

		[Fact]
		public void GetStudents_Returns200OK_WhenDBIsEmpty()
		{
			//Arrange 
			mockRepo.Setup(repo =>
			   repo.GetAllStudents()).Returns(GetStudents(0));

			var studentsController = new StudentsController(mockRepo.Object, mapper);

			//Act
			var result = studentsController.GetAllStudents();

			//Assert
			Assert.IsType<OkObjectResult>(result.Result);	
		}

		[Fact]
		public void GetStudents_ReturnsSingle_WhenDBHasOne()
		{
			//Arrange 
			mockRepo.Setup(repo =>
			   repo.GetAllStudents()).Returns(GetStudents(1));

			var studentsController = new StudentsController(mockRepo.Object, mapper);

			//Act
			var result = studentsController.GetAllStudents();

			//Assert
			var okResult = result.Result as OkObjectResult;
			var students = okResult.Value as List<StudentReadDto>;
			Assert.Single(students);
		}

		[Fact]
		public void GetStudents_Response200OK_WhenDBHasOne()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetAllStudents()).Returns(GetStudents(1));
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.GetAllStudents();

			//Assert
			Assert.IsType<OkObjectResult>(result.Result);
		}	
		
		[Fact]
		public void GetStudents_ReturnsCorrectType_WhenDBHasOne()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetAllStudents()).Returns(GetStudents(1));
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.GetAllStudents();

			//Assert
			Assert.IsType<ActionResult<IEnumerable<StudentReadDto>>>(result);
		}
		
		[Fact]
		public void GetStudentsById_Returns404NotFound_WhenInappropriateID()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(0)).Returns(() => null);
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.GetStudentById(1);

			//Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public void GetStudentsById_Returns200OK_WhenAppropriateID()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(1)).Returns(new Student
				{
					IndexNum = "111111",
					IsCheater = false,
					AccessCode = "asda",
					Points = 20,
					Mark = 5,
					StartDate = "2020-01-20",
					FinishDate = "2020-01-20"
					});

			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.GetStudentById(1);

			//Assert
			Assert.IsType<OkObjectResult>(result.Result);
		}	
		
		[Fact]
		public void GetStudentsById_CorrectType_WhenAppropriateID()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(1)).Returns(new Student
				{
					IndexNum = "111111",
					IsCheater = false,
					AccessCode = "asda",
					Points = 20,
					Mark = 5,
					StartDate = "2020-01-20",
					FinishDate = "2020-01-20"
					});

			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.GetStudentById(1);

			//Assert
			Assert.IsType<ActionResult<StudentReadDto>>(result);
		}

		[Fact]
		public void CreateStudent_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(1)).Returns(new Student
				{
					IndexNum = "111111",
					IsCheater = false,
					AccessCode = "asda",
					Points = 20,
					Mark = 5,
					StartDate = "2020-01-20",
					FinishDate = "2020-01-20"
				});
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.AddStudent(new StudentCreateDto { });
			//Assert
			Assert.IsType<ActionResult<StudentReadDto>>(result);
		}
		[Fact]
		public void CreateStudent_Returns201Created_WhenValidObjectSubmitted()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(1)).Returns(new Student
				{
					IndexNum = "111111",
					IsCheater = false,
					AccessCode = "asda",
					Points = 20,
					Mark = 5,
					StartDate = "2020-01-20",
					FinishDate = "2020-01-20"
				});
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.AddStudent(new StudentCreateDto { });
			//Assert
			Assert.IsType<CreatedAtRouteResult>(result.Result);
		}
		[Fact]
		public void UpdateStudent_Returns204NoContent_WhenValidObjectSubmitted()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(1)).Returns(new Student
				{
					IndexNum = "111111",
					IsCheater = false,
					AccessCode = "asda",
					Points = 20,
					Mark = 5,
					StartDate = "2020-01-20",
					FinishDate = "2020-01-20"
				});
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.UpdateStudent(new StudentUpdateDto { },1);
			//Assert
			Assert.IsType<NoContentResult>(result.Result);
		}
		[Fact]
		public void UpdateStudent_Returns404NotFound_WhenInvalidIDSubmitted()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(0)).Returns(() => null);
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.UpdateStudent(new StudentUpdateDto { },0);
			//Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}
		
		[Fact]
		public void PatchStudent_Returns404NotFound_WhenInvalidIDSubmitted()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(0)).Returns(() => null);
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.UpdateStudentPartially(0, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<StudentUpdateDto> { });
			//Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}
		[Fact]
		public void DeleteStudent_Returns204NoContent_WhenValidIDSubmitted()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(1)).Returns(new Student
				{
					IndexNum = "111111",
					IsCheater = false,
					AccessCode = "asda",
					Points = 20,
					Mark = 5,
					StartDate = "2020-01-20",
					FinishDate = "2020-01-20"
				});
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.DeleteStudent(1);
			//Assert
			Assert.IsType<NoContentResult>(result);
		}
		[Fact]
		public void DeleteStudent_Returns404NotFound_WhenInvalidIDSubmitted()
		{
			//Arrange
			mockRepo.Setup(repo =>
				repo.GetStudent(0)).Returns(() => null);
			var studentsController = new StudentsController(mockRepo.Object, mapper);
			//Act
			var result = studentsController.DeleteStudent(0);
			//Assert
			Assert.IsType<NotFoundResult>(result);
		}


		private List<Student> GetStudents(int num)
		{
			var students = new List<Student>();
			if (num > 0)
			{
				students.Add( new Student
				{
					IndexNum = "111111",
					IsCheater = false,
					AccessCode = "asda",
					Points = 20,
					Mark = 5,
					StartDate = "2020-01-20",
					FinishDate = "2020-01-20"
				});
			}

			return students;
		}
	}
}
