using Server_WebAPI.Tests.Common;
using ServerAPI.Controllers;
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
    public class EmailControllerTests : TestBase
    {
        private readonly IEmailRepo _emailRepo;
        private readonly EmailController _controller;
        private readonly EmailSettings _emailSettings;
        private readonly IEmployeeUserRepo _employeeUserRepo;


        public EmailControllerTests() 
            : base()
        {
            _emailRepo = new EmailService(_emailSettings,_context);
            _controller = new EmailController(_emailRepo,_employeeUserRepo);
        }
    }
}
