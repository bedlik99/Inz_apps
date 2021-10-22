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
		public EmailController(IEmailRepo emailService)
		{
			_emailService = emailService;
		}

		[HttpPost]
		public ActionResult<string> SendEmail(EmailData emailData)
		{
			return Ok(_emailService.SendEmail(emailData));
		}
	}
}
