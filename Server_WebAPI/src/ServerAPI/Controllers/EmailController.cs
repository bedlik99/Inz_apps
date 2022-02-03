using Microsoft.AspNetCore.Mvc;
using ServerAPI.Entities;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("/email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepo _emailService;
        private readonly IEmployeeUserRepo _employeeUserRepo;
        public EmailController(IEmailRepo emailService, IEmployeeUserRepo employeeUserRepo)
        {
            _emailService = emailService;
            _employeeUserRepo = employeeUserRepo;
        }

        /// <summary>
        /// Endpoint odbierający żądanie wysłania pojedynczej wiadomości email.
        /// </summary>
        /// <param name="emailData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> SendEmail(EmailData emailData)
        {
            return Ok(_emailService.SendEmail(emailData));
        }

        /// <summary>
        /// Endpoint odbierający żądanie wysłania wiadomości email nt. wskazanego laboratorium w celach testowych. 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="labName"></param>
        /// <returns></returns>
        [Route("db")]
        [HttpPost]
        public ActionResult<string> SendEmailDbData([FromQuery] string email, [FromQuery] string labName)
        {
            var user = _employeeUserRepo.GetRegisteredUser(email, labName);
            return Ok(_emailService.SendEmailDbData(user));
        }
    }
}