using Microsoft.AspNetCore.Mvc;
using ServerAPI.DTOs;
using ServerAPI.Entities;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
	//Setting up a routing
	[Route("api/")]
	//This provides: Routing, Automatic HTTP 400 Errors, Details
	[ApiController]
	public class LabUserController : ControllerBase
	{
		private readonly IEitiLabUserRepo _repository;
		public LabUserController(IEitiLabUserRepo repository)
		{
			_repository = repository;
		}

		[Route("registerUser")]
		[HttpPost]
		public ActionResult<string> RegisterUser([FromBody] MessageDTO encryptedMessage)
		{
			var result = _repository.ProcessUserInitData(encryptedMessage);
			if(!result)
			{	
				return Unauthorized("Could not authenticate user. Check your credentials and try!");
			}
			return Ok();
		}

		[Route("recordEvent")]
		[HttpPost]
		public ActionResult RecordEvent([FromBody] MessageDTO encryptedMessage)
		{
			var success = _repository.ProcessEventContent(encryptedMessage);
			if(!success)
				return BadRequest("Could not record an event");
			return Ok();
		}
	}
}
