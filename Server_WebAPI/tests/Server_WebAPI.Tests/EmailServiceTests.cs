using Microsoft.Extensions.Configuration;
using Server_WebAPI.Tests.Common;
using ServerAPI.Entities;
using ServerAPI.Repositories;
using ServerAPI.Services;
using ServerAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Server_WebAPI.Tests
{
    public class EmailServiceTests : TestBase
    {
        private readonly IEmailRepo _service;
        private readonly EmailSettings _emailSettings;

        public EmailServiceTests() : base()
        {
            _emailSettings = new EmailSettings()
            {
                EmailId = "testingforwebdev@gmail.com",
                Name = "EiTI Lab Serwis",
                Password = "Testing123!",
                Host = "smtp.gmail.com",
                Port = 587,
                UseSSL = true
            };
            _service = new EmailService(_emailSettings, _context);
        }

        [Fact]
        public void SendEmail_ForSpecifiedData_SendsEmail()
        {
            EmailData data = new EmailData()
            {
                EmailBody = "TestBase",
                EmailSubject = "Test",
                EmailToId = "testingforwebdev@gmail.com",
                EmailToName = "TEST"
            };
            var expection = $"An email has been sent to {data.EmailToId}";
            var str = _service.SendEmail(data);

            Assert.Equal(expection, str);
        }
    }
}
