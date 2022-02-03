using Server_WebAPI.Tests.Common;
using ServerAPI.DTOs;
using ServerAPI.Entities;
using ServerAPI.Repositories;
using ServerAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Server_WebAPI.Tests
{
    public class EmployeeUserServiceTests : TestBase
    {
        private readonly IEmployeeUserRepo _service;
        public EmployeeUserServiceTests()
            : base()
        {
            _service = new EmployeeUserService(_context);
        }

        // Arrange
        public string labName = "PKC";
        public string email = "testingforwebdev@gmail.com";

        [Fact]
        public void GetRegisteredUser_WhenValidNameAndMail_ReturnsRegisteredUser()
        {
            // Act
            var result = _service.GetRegisteredUser(email, labName);

            //Asert
            Assert.IsType<RegisteredUser>(result);
        }

        [Fact]
        public void GetUserRegistries_WhenValidNameAndMail_ReturnUserRegistries()
        {
            // Act
            var result = _service.GetUserRegistries(email, labName);

            //Asert
            Assert.IsType<List<RecordedEventDTO>>(result);
        }

        [Fact]
        public void GetUserRegistries_WhenValidNameAndMail_ReturnsOne()
        {
            var occurences = 1;

            // Act
            var result = _service.GetUserRegistries(email, labName);

            //Asert
            Assert.Equal(occurences, result.Count());
        }

        [Fact]
        public void GetAllRegisteredUsers_WhenCalled_ReturnsNonEmptyCollection()
        {
            // Act
            var collection = _service.GetAllRegisteredUsers().ToList();
            //Assert
            Assert.NotEmpty(collection);
        }

        [Fact]
        public void GetAllRegisteredUsers_WhenValidNameAndMail_ReturnsTwo()
        {
            var occurences = 2;

            // Act
            var result = _service.GetAllRegisteredUsers();

            //Asert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetUsersByLab_WhenLabProvided_ReturnsUsers()
        {
            // Act
            var result = _service.GetUsersByLab(labName).ToList();

            //Assert
            Assert.Equal(email, result.First().Email);
        }

        [Fact]
        public void DeleteRegisteredUser_WhenInputIsValid_ReturnTrue()
        {
            var output = true;
            // Act
            var result = _service.DeleteRegisteredUser(email, labName);

            //Assert
            Assert.Equal(output, result);
        }

        [Fact]
        public void InsertUsersIntoDataBase_WithValidUserList_ReturnSetOfRegisteredUserDto()
        {
            //Arrange
            var userList = new List<RegisteredUser>()
            {
                new RegisteredUser
                {
                    Email = "01122331@pw.edu.pl",
                    Laboratory = _context.LaboratoryItems.SingleOrDefault(l=>l.LabName == labName)
                }
            };

            // Act
            var result = _service.InsertUsersIntoDataBase(userList);

            //Assert
            Assert.IsType<HashSet<RegisteredLabUserDTO>>(result);
        }

        [Fact]
        public void InsertUsersIntoDataBase_WithValidUserList_SuccessfulAddition()
        {
            //Arrange
            var userList = new List<RegisteredUser>()
            {
                new RegisteredUser
                {
                    Email = "01122331@pw.edu.pl",
                    Laboratory = _context.LaboratoryItems.SingleOrDefault(l=>l.LabName == labName)
                }
            };
            var number = 1;
            // Act
            var result = _service.InsertUsersIntoDataBase(userList);
            var resultNumber = result.Count();

            //Assert
            Assert.True(number == resultNumber);
        }

        [Fact]
        public void InsertUsersIntoDataBase_WithRepeatingStudent_ReturnEmptySet()
        {
            //Arrange
            var userList = new List<RegisteredUser>()
            {
                new RegisteredUser
                {
                    Email = email,
                    Laboratory = _context.LaboratoryItems.SingleOrDefault(l=>l.LabName == labName)
                }
            };
            ;
            // Act
            var result = _service.InsertUsersIntoDataBase(userList);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CheckUniquenessOfCode_UserHasUniqueCode_ReturnDifferentUniqueCode()
        {

            var user = _context.RegisteredUserItems.SingleOrDefault(x => x.Email == email);
            var alreadyUsedUniqueCode = user.UniqueCode;

            // Act
            var result = _service.CheckUniquenessOfCode(user);

            //Assert
            Assert.NotEqual(alreadyUsedUniqueCode, result);
        }

        [Fact]
        public void GenerateResults_ForNotExistingLab_ReturnNull()
        {

            // Act
            var result = _service.GenerateResults("BADLAB");

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GenerateResults_ForExistingLab_ReturnProperBuilder()
        {
            DateTime? newdate = new DateTime();
            var builder = new StringBuilder();
            builder.AppendLine($"{email};{labName};False;{newdate}");

            // Act
            var result = _service.GenerateResults(labName);

            //Assert
            Assert.Contains(builder.ToString(), result.ToString());
        }

        [Fact]
        public void GenerateResult_ForExistingLab_ReturnProperBuilder()
        {
            DateTime? newdate = new DateTime();
            var builder = new StringBuilder();
            builder.AppendLine($"{email};{labName};False;{newdate};");

            // Act
            var result = _service.GenerateResult(labName, email);

            //Assert
            Assert.Contains(builder.ToString(), result.ToString());
        }

        [Fact]
        public void DeleteLaboratoryAndUsersData_ForExistingLab_ReturnTrue()
        {
            var result = _service.DeleteLaboratoryAndUsersData(labName);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteLaboratoryAndUsersData_ForOnlyOneLabInContext_WillBeNull()
        {
            var num = _context.LaboratoryItems.Count();
            _service.DeleteLaboratoryAndUsersData(labName);
            var after = _context.LaboratoryItems.Count();
            //Assert
            Assert.True(after<num);
        }

        [Fact]
        public void GetLabRequirements_ForValidInput_WillReturnListOfRequirements()
        {
            var list = _service.GetLabRequirements(labName);

            //Assert
            Assert.IsType<List<LaboratoryRequirement>>(list);
        }

        [Fact]
        public void GetLabRequirements_ForInValidInput_WillReturnNull()
        {
            var result = _service.GetLabRequirements("Lab");
            //Assert
            Assert.Null(result);
        }
    }
}
