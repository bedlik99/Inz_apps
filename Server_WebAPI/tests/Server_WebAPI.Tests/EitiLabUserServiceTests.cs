using Server_WebAPI.Tests.Common;
using ServerAPI.DTOs;
using ServerAPI.Entities;
using ServerAPI.Exceptions;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Server_WebAPI.Tests
{
    public class EitiLabUserServiceTests : TestBase
    {
        private readonly IEitiLabUserRepo _service;
        public EitiLabUserServiceTests() : base()
        {
            _service = new EitiLabUserService(_context);
        }
        // Arrange
        public string labName = "PKC";
        public string email = "testingforwebdev@gmail.com";

        [Fact]
        public void ProcessUserInitData_ForProperEncryptedMessage_ReturnsTrue()
        {
            var user = _context.RegisteredUserItems.Single(x => x.Email == "88888888@pw.edu.pl");
            var input = "{\"email\":\"" +
                          user.Email +
                          "\",\"uniqueCode\":\"" +
                          user.UniqueCode +
                          "\"}";

            var encryptedMessage = new MessageDTO { Value = CryptographyTesting.Program.EncryptionUserAndEvent(input) };


            var result = _service.ProcessUserInitData(encryptedMessage);

            Assert.True(result);
        }
        
        [Fact]
        public void ProcessEventContent_ForProperEncryptedMessage_ReturnsTrue()
        {
            var user = _context.RegisteredUserItems.Single(x => x.Email == "88888888@pw.edu.pl");
            var input = "{\"registryContent\":\"" +
                         "bash_command>ls" +
                          "\",\"uniqueCode\":\"" +
                          user.UniqueCode +
                          "\"}";

            var encryptedMessage = new MessageDTO { Value = CryptographyTesting.Program.EncryptionUserAndEvent(input) };


            var result = _service.ProcessEventContent(encryptedMessage);

            Assert.True(result);
        }
        
        [Fact]
        public void FindStudentByUniqueCode_ValidUniqueCode_ReturnsUser()
        {
            var uniqueCode = _context.RegisteredUserItems.SingleOrDefault(x=>x.Email==email).UniqueCode;

            var result = _service.FindStudentByUniqueCode(uniqueCode);

            Assert.IsType<RegisteredUser>(result);
        }
        

        [Fact]
        public void FindStudentByUniqueCode_NotValidUniqueCode_ThrowsException()
        {
            var uniqueCode = "12345678";

            Action action = () => _service.FindStudentByUniqueCode(uniqueCode);

            Assert.Throws<NotAcceptableException>(action);
        }

        [Fact]
        public void ValidateUserData_WhenValidData_ReturnsRegisteredUser()
        {
            var validData = new RegisteredLabUserDTO
            {
                Email = "88888888@pw.edu.pl",
                UniqueCode = _context.RegisteredUserItems.SingleOrDefault(x => x.Email == "88888888@pw.edu.pl").UniqueCode
            };

            var s = new EitiLabUserService(_context); 
            var result = s.ValidateUserData(validData);

            Assert.IsType<RegisteredUser>(result);
        }

        [Fact]
        public void ValidateUserData_WhenInvalidData_ReturnsNull()
        {
            var validData = new RegisteredLabUserDTO
            {
                Email = email,
                UniqueCode = _context.RegisteredUserItems.SingleOrDefault(x => x.Email == email).UniqueCode
            };

            var s = new EitiLabUserService(_context); 
            var result = s.ValidateUserData(validData);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("abcde")]
        [InlineData("abcdefghijklmnoprsta")]
        [InlineData("abcdefghijklmnoprstaweasdaaaaaaaaaaa")]

        public void FindMaxRangeOfStringLength_ForDifferentStringLenghts_ProducesZeroModulo16(string str)
        {
            var s = new EitiLabUserService(_context);
            var result = s.FindMaxRangeOfStringLength(str.Length, 0, 16);

            Assert.True(result % 16 == 0);
        } 
        
        [Theory]
        [InlineData("1properstring")]
        [InlineData("2aproperstring")]
        [InlineData("a123456789properstring")]

        public void RemoveCharsFromString_ForDifferentHex_RemovesChars(string str)
        {
            var properstring = "properstring";
            var s = new EitiLabUserService(_context);
            var result = s.RemoveCharsFromString(str);

            Assert.Equal(properstring, result); 
        }

        [Fact]
        public void DecryptMessage_ForProperMessageAndRegistration_DecryptProperly()
        {
            var user = _context.RegisteredUserItems.Single(x => x.Email == "88888888@pw.edu.pl");
            var input = "{\"email\":\"" +
                          user.Email +
                          "\",\"uniqueCode\":\"" +
                          user.UniqueCode +
                          "\"}";

            var encryptedMessage = CryptographyTesting.Program.EncryptionUserAndEvent(input);
            
            var s = new EitiLabUserService(_context);

            var result = (RegisteredLabUserDTO)s.DecryptMessage(encryptedMessage, true);

            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public void DecryptMessage_ForProperMessageAndEventContent_DecryptProperly()
        {
            var user = _context.RegisteredUserItems.Single(x => x.Email == "88888888@pw.edu.pl");
            var input = "{\"registryContent\":\"" +
                         "bash_command>ls" +
                          "\",\"uniqueCode\":\"" +
                          user.UniqueCode +
                          "\"}";

            var encryptedMessage = CryptographyTesting.Program.EncryptionUserAndEvent(input);
            
            var s = new EitiLabUserService(_context);

            var result = (RecordedEventDTO)s.DecryptMessage(encryptedMessage, false);

            Assert.Equal(user.UniqueCode, result.UniqueCode);
        }

        [Fact]
        public void GetExecutedCommand_CreatedRecorededEvent_GetsRegistryContent()
        {
            var validData = new RecordedEventDTO
            {
                UniqueCode = _context.RegisteredUserItems.SingleOrDefault(x => x.Email == "88888888@pw.edu.pl").UniqueCode,
                RegistryContent = "bash_command>ls"
            };

            var expected = "ls";
            
            var s = new EitiLabUserService(_context);

            var result = s.GetExecutedCommand(validData);

            Assert.Equal(expected,result);
        }

        [Fact]
        public void AddExecutedCommand_ForExistingUser_AddsExecutedCommand()
        {
            var user = _context.RegisteredUserItems.FirstOrDefault(x=>x.Email == email);
            var expected = "ls";
            
            var s = new EitiLabUserService(_context);

            s.AddExecutedCommand(user,expected);

            var result = user.ExecutedCommands.Count();

            Assert.Equal(1,result);
        }

        [Fact]
        public void CheckIfCompletedRequirements_UserWithExecutedCommands_SetsCompletedFlag()
        {
            var user = _context.RegisteredUserItems.FirstOrDefault(x => x.Email == email);
            var expected = "ls";
            user.ExecutedCommands.Add(new ExecutedCommand {ExecutionDate = DateTime.Now, Content = expected });

            var s = new EitiLabUserService(_context);

            s.CheckIfCompletedRequirements(user);

            var result = user.NoWarning;

            Assert.True(result);
        }
    }
}
