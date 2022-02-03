using Microsoft.AspNetCore.Mvc;
using ServerAPI.DTOs;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
	[Route("api/")]
	[ApiController]
	public class LabUserController : ControllerBase
	{
		private readonly IEitiLabUserRepo _repository;
		public LabUserController(IEitiLabUserRepo repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Endpoint rejestracji użytkownika przyjmujący zaszyfrowaną wartość z maszny klienckiej.
		/// </summary>
		/// <param name="encryptedMessage"></param>
		/// <returns></returns>
		[Route("registerUser")]
		[HttpPost]
		public ActionResult RegisterUser([FromBody] 
			MessageDTO 	encryptedMessage)
		{
			var result = _repository.
				ProcessUserInitData(encryptedMessage);
			if (!result)
			{
				return Unauthorized();
			}
			return Ok();
		}

		/// <summary>
		/// Endpoint przyjmujący zaszyfrowaną wartość z maszny klienckiej, w której znajduje się opis wykonanej operacji na maszynie klienckiej.
		/// </summary>
		/// <param name="encryptedMessage"></param>
		/// <returns></returns>
		[Route("recordEvent")]
		[HttpPost]
		public ActionResult RecordEvent([FromBody] 
			MessageDTO 	encryptedMessage)
		{
			var success = _repository.
				ProcessEventContent(encryptedMessage);
			if (!success)
				return BadRequest();
			return Ok();
		}
	}
}

