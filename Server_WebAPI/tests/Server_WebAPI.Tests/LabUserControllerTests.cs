using Microsoft.AspNetCore.Mvc;
using Server_WebAPI.Tests.Common;
using ServerAPI.Controllers;
using ServerAPI.DTOs;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Server_WebAPI.Tests
{
    public class LabUserControllerTests : TestBase
    {
        private readonly IEitiLabUserRepo _repository;
        private readonly LabUserController _controller;

        public LabUserControllerTests()
            : base()
        {
            _repository = new EitiLabUserService(_context);
            _controller = new LabUserController(_repository);
        }

        [Fact]
        public void RegisterUser_ForEncryptedMessage_Returns200OK()
        {
            //Arrange
            var user = _context.RegisteredUserItems.Single(x => x.Email == "88888888@pw.edu.pl");
            var input = "{\"email\":\"" +
                          user.Email +
                          "\",\"uniqueCode\":\"" +
                          user.UniqueCode +
                          "\"}";

            var encryptedMessage = CryptographyTesting.Program.EncryptionUserAndEvent(input);
            var message = new MessageDTO { Value = encryptedMessage };

            //Act
            var result = _controller.RegisterUser(message);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void RecordEvent_ForEncryptedMessage_Returns200OK()
        {
            //Arrange
            var user = _context.RegisteredUserItems.Single(x=> x.Email == "88888888@pw.edu.pl");
            var input = "{\"registryContent\":\"" +
                         "bash_command>ls" +
                          "\",\"uniqueCode\":\"" +
                          user.UniqueCode +
                          "\"}";

            var encryptedMessage = CryptographyTesting.Program.EncryptionUserAndEvent(input);
            var message = new MessageDTO { Value = encryptedMessage };

            //Act
            var result = _controller.RecordEvent(message);

            //Assert
            Assert.IsType<OkResult>(result);
        }

    }
}
