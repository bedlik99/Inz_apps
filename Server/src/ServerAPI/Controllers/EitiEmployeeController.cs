using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.DTOs;
using ServerAPI.Entities;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
	[Route("/")]
	[ApiController]
	[Authorize]
	public class EitiEmployeeController : ControllerBase
	{
		private readonly IEmployeeUserRepo _employeeUserRepo;
		private readonly IEmailRepo _emailService;
		public EitiEmployeeController(IEmployeeUserRepo employeeUserRepo, IEmailRepo emailRepo)
		{
			_employeeUserRepo = employeeUserRepo;
			_emailService = emailRepo;
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("/register")]
		public ActionResult RegisterEmployee(RegisteredEmployeeDto registeredEmployeeDto)
		{
			_employeeUserRepo.RegisterEmployee(registeredEmployeeDto);
			return Ok();
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("/login")]
		public ActionResult Login([FromBody] LoginDto loginDto)
		{
			string token = _employeeUserRepo.GenerateJwt(loginDto);
			return Ok(token);
		}

		//************************ CRUD OPERATIONS ************************
		[HttpGet]
		[AllowAnonymous]
		[Route("users")]
		public ActionResult<IEnumerable<RegisteredUser>> GetAllRegisteredUsers()
		{
			var output = _employeeUserRepo.GetAllRegisteredUsers();
			if (output == null)
				return BadRequest();
			return Ok(output);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("users/lab")]
		public ActionResult<RegisteredUser> GetUsersByLab([FromQuery] string labName)
		{
			var output = _employeeUserRepo.GetUsersByLab(labName);
			if (output == null)
				return BadRequest();
			return Ok(output);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("user")]
		public ActionResult<RegisteredUser> GetSingleRegisteredUser([FromQuery] string mail)
		{
			var output = _employeeUserRepo.GetRegisteredUser(mail);
			if (output == null)
				return BadRequest();
			return Ok(output);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("user/registries")]
		public ActionResult<RegisteredUser> GetUserRegistries([FromQuery] string mail)
		{
			var output = _employeeUserRepo.GetUserRegistries(mail);
			if (output == null)
				return BadRequest();
			return Ok(output);
		}

		[HttpDelete]
		[AllowAnonymous]
		[Route("user")]
		//[Authorize(Roles = "Administrator")]
		public ActionResult DeleteRegisteredUser([FromQuery] string mail)
		{
			var result = _employeeUserRepo.DeleteRegisteredUser(mail);
			if (!result)
				return BadRequest();
			return Ok();
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("employees")]
		public ActionResult<IEnumerable<RegisteredUser>> GetAllRegisteredEmployees()
		{
			var output = _employeeUserRepo.GetAllRegisteredEmployees();
			if (output == null)
				return BadRequest();
			return Ok(output);
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("upload")]
		public ActionResult UploadStudnetFile ([FromForm] IFormFile fileUpload, [FromQuery] string owner)
		{
			var labName = fileUpload.FileName.Split(".")[0];
			var uploadResult = _employeeUserRepo.UploadFileService(fileUpload,labName,owner);
			var set = _employeeUserRepo.InsertUsersIntoDataBase(uploadResult);
			if (!set.Any())
			{
				return BadRequest();
			}
			return Ok(_emailService.SendEmail(set));
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("requirements")]
		public ActionResult UploadLabRequirements([FromForm] IFormFile fileUpload)
		{
			var labName = string.Join("_",fileUpload.FileName.Split("_").Take(2));
			var uploadResult = _employeeUserRepo.UploadLabRequirements(fileUpload,labName);
			if (!uploadResult)
			{
				return BadRequest("Dodawanie wymagań nie powiodło się. Sprawdź poprawność zapytania.");
			}
			return Ok();
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("requirements")]
		public ActionResult<IEnumerable<LaboratoryRequirement>> GetLabRequirements([FromQuery] string labName)
		{
			var result = _employeeUserRepo.GetLabRequirements(labName);
			if (result == null)
				return BadRequest();
			return Ok(result);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("results")]
		public ActionResult GenerateResults([FromQuery] string labName)
		{
			var builder = _employeeUserRepo.GenerateResults(labName);
			if (builder == null)
				return BadRequest();
			return File(Encoding.UTF8.GetBytes(builder.ToString()),"text/csv",$"{labName}_Results.csv");;
		}

		//TBC
		[HttpDelete]
		[AllowAnonymous]
		[Route("delete/lab")]
		public ActionResult DeleteLaboratoryAndUsersData([FromQuery] string labName)
		{
			var result = _employeeUserRepo.DeleteRegisteredUser(labName);
			if (!result)
				return BadRequest();
			return Ok();
		}
	}
}

