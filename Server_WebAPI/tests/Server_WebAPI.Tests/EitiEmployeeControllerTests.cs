using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server_WebAPI.Tests.Common;
using ServerAPI.Controllers;
using ServerAPI.Entities;
using ServerAPI.Repositories;
using ServerAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Server_WebAPI.Tests
{
    public class EitiEmployeeControllerTests : TestBase
    {
        private readonly EitiEmployeeController _controller;
        private readonly IEmployeeUserRepo _employeeUserRepo;
        private readonly IEmailRepo _emailService;

        public EitiEmployeeControllerTests()
            : base()
        {
            _employeeUserRepo = new EmployeeUserService(_context);
            _controller = new EitiEmployeeController(_employeeUserRepo, _emailService);
        }

        //Schema: NazwaMetody_Scenariusz_Rezultat

        [Fact]
        public void GetAllRegisteredUsers_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetAllRegisteredUsers();
            // Assert
            var result = okResult.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAllRegisteredUsers_WhenCalled_ReturnsAllUsers()
        {

            //Act
            var okResult = _controller.GetAllRegisteredUsers();

            //Assert
            var result = okResult.Result as OkObjectResult;
            var resultRegisteredUsers = result.Value as IEnumerable<RegisteredUser>;

        }

        [Fact]
        public void GetUsersByLab_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var labCorrect = "PKC";
            var labInvalid = "NO_PKC";
            // Act
            var okResult = _controller.GetUsersByLab(labCorrect);
            var notFoundResult = _controller.GetUsersByLab(labInvalid);
            var resultPKC = okResult.Result as OkObjectResult;
            var resultNOPKC = notFoundResult.Result as NotFoundResult;

            //Assert
            Assert.IsType<OkObjectResult>(resultPKC);
            Assert.IsType<NotFoundResult>(resultNOPKC);
        }

        [Fact]
        public void DeleteLaboratoryAndUsersData_WithLabNamePassed_RemovesLaboratoryAndData()
        {
            // Arrange
            var labName = "PKC";
            var noLab = "NOPKC";
            // Act
            var okResult = _controller.DeleteLaboratoryAndUsersData(labName);
            var badRequest = _controller.DeleteLaboratoryAndUsersData(noLab);

            //Assert
            Assert.IsType<OkResult>(okResult);
            Assert.IsType<BadRequestResult>(badRequest);
        }

        [Fact]
        public void GenerateResult_ForUserAndLabPassed_GenerateFile()
        {
            // Arrange
            var labName = "PKC";
            var noLab = "NOPKC";
            var email = "testingforwebdev@gmail.com";

            // Act
            var fileContentResult = _controller.GenerateResult(labName,email);
            var badRequest = _controller.GenerateResult(noLab, email);

            //Assert
            Assert.IsType<FileContentResult>(fileContentResult);
            Assert.IsType<BadRequestResult>(badRequest);
        }

        [Fact]
        public void GenerateResults_ForLabPassed_GenerateFile()
        {
            // Arrange
            var labName = "PKC";

            // Act
            var fileContentResult = _controller.GenerateResults(labName);

            //Assert
            Assert.IsType<FileContentResult>(fileContentResult);
        }

        [Fact]
        public void GetLabRequirements_ForGivenLabName_Return200OK()
        {
            // Arrange
            var labName = "PKC";

            // Act
            var okResult = _controller.GetLabRequirements(labName).Result;

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetAllRegisteredEmployees_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetAllRegisteredEmployees();
            var result = okResult.Result as OkObjectResult;
           
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteRegisteredUser_ClearListForLabAndUser_ReturnsOkResult()
        {
            //Arrange
            var student = "testingforwebdev@gmail.com";
            var labName = "PKC";

            // Act
            var okResult = _controller.DeleteRegisteredUser(student,labName);
            // Assert
            Assert.IsType<OkResult>(okResult);
        }


        [Fact]
        public void GetUserRegistries_ForPassedMailAndLab_ReturnsOkResult()
        {
            //Arrange
            var student = "testingforwebdev@gmail.com";
            var labName = "PKC";

            // Act
            var okResult = _controller.GetUserRegistries(student, labName).Result;
            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetSingleRegisteredUser_ForPassedMailAndLab_ReturnsOkResult()
        {
            //Arrange
            var student = "testingforwebdev@gmail.com";
            var labName = "PKC";

            // Act
            var okResult = _controller.GetSingleRegisteredUser(student, labName).Result;
            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }


        //[Fact]
        //public void UploadLabRequirements_ForSomeFile_Return200OK()
        //{
        //    var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        //    IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

        //    // Act
        //    var okResult = _controller.UploadLabRequirements(file);

        //    //Assert
        //    Assert.IsType<OkResult>(okResult);
        //}

        //[Fact]
        //public void UploadLabRequirements_ForSomeFile_Return200OK()
        //{
        //    var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        //    IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

        //    // Act
        //    var okResult = _controller.UploadLabRequirements(file);

        //    //Assert
        //    Assert.IsType<OkResult>(okResult);
        //}
    }
}
