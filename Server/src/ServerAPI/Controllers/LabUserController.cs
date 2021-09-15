using Microsoft.AspNetCore.Mvc;
using ServerAPI.DTOs;
using ServerAPI.Models;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
	//Setting up a routing
	[Route("/")]
	//This provides: Routing, Automatic HTTP 400 Errors, Details,
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
		public ActionResult<RegisteredUserDTO> RegisterUser([FromBody] MessageDTO encryptedMessage)
		{
			var result = _repository.ProcessUserInitData(encryptedMessage);
			if(result == null)
			{
				return BadRequest();
			}
			return Ok();
		}

		[Route("recordEvent")]
		[HttpPost]
		public ActionResult RecordEvent([FromBody] MessageDTO encryptedMessage)
		{
			_repository.ProcessEventContent(encryptedMessage);
			return Ok();
		}
		
		[HttpGet]
		[Route("user")]
		public ActionResult<IEnumerable<RegisteredUser>> GetAllRegisteredUsers()
		{
			var output = _repository.GetAllRegisteredUsers();
			return Ok(output);
		}

		[HttpGet]
		[Route("user/{id}")]
		public ActionResult<RegisteredUser> GetRegisteredUser([FromRoute]int id)
		{
			var output = _repository.GetRegisteredUser(id);
			return Ok(output);
		}
	}
}
