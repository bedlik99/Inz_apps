using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAPI.Data;
using StudentAPI.Models;
using AutoMapper;
using StudentAPI.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace StudentAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		//Dependency Injection is used to make these available for Controller
		//Fields created to hold instances of Mapper and Repo
		private readonly IStudentAPIRepo _studentAPIRepo;
		private readonly IMapper _mapper;

		//Dependency Injection happens here 
		public StudentsController(IStudentAPIRepo studentAPIRepo, IMapper mapper)
		{
			_studentAPIRepo = studentAPIRepo;
			_mapper = mapper;
		}
		//GET Controller - Get list of students
		[HttpGet]
		public ActionResult<IEnumerable<StudentReadDto>> GetAllStudents()
		{
			var students = _studentAPIRepo.GetAllStudents();
			return Ok(_mapper.Map<IEnumerable<StudentReadDto>>(students));
		}

		//GET{id} Controller - Get one of the students
		[HttpGet("{id}", Name = "GetStudentById")]
		public ActionResult<StudentReadDto> GetStudentById(int id)
		{
			var student = _studentAPIRepo.GetStudent(id);
			if (student == null)
			{
				return NotFound();
			}
			//Mapping into an object (student) into the format <> 
			return Ok(_mapper.Map<StudentReadDto>(student));
		}

		//POST Controller - Add new student to DB
		[HttpPost]
		public ActionResult<StudentReadDto> AddStudent(StudentCreateDto studentCreateDto)
		{
			var studentModel = _mapper.Map<Student>(studentCreateDto);
			_studentAPIRepo.AddStudent(studentModel);
			_studentAPIRepo.SaveChanges();

			var studentReadDto = _mapper.Map<StudentReadDto>(studentModel);

			return CreatedAtRoute(nameof(GetStudentById), new { Id = studentReadDto.Id }, studentReadDto);
		}

		//PUT Controller - Update whole student object
		[HttpPut("{id}")]
		public ActionResult<StudentReadDto> UpdateStudent(StudentUpdateDto studentUpdateDto, int id)
		{
			var studentRepo = _studentAPIRepo.GetStudent(id);
			if (studentRepo == null)
			{
				return NotFound();
			}

			_mapper.Map(studentUpdateDto, studentRepo);
			_studentAPIRepo.UpdateStudent(studentRepo);
			_studentAPIRepo.SaveChanges();
			return NoContent();
		}
		//PATCH Controller - Update partialy student object
		[HttpPatch("{id}")]
		public ActionResult<StudentReadDto> UpdateStudentPartially(int id, JsonPatchDocument<StudentUpdateDto> patchDoc)
		{
			var studentRepo = _studentAPIRepo.GetStudent(id);
			if (studentRepo == null)
			{
				return NotFound();
			}

			var studentToPatch = _mapper.Map<StudentUpdateDto>(studentRepo);
			patchDoc.ApplyTo(studentToPatch, ModelState);

			if (!TryValidateModel(studentToPatch))
			{
				return ValidationProblem(ModelState);
			}
			//apply patch document received in request body to the newly created 
			_mapper.Map(studentToPatch, studentRepo);
			_studentAPIRepo.UpdateStudent(studentRepo);
			_studentAPIRepo.SaveChanges();
			return NoContent();
		}
		//DELETE Controller - Delete specific student object
		[HttpDelete("{id}")]
		public ActionResult DeleteStudent(int id)
		{
			var studentRepo = _studentAPIRepo.GetStudent(id);

			if (studentRepo == null)
			{
				return NotFound();
			}
			_studentAPIRepo.DeleteStudent(studentRepo);
			_studentAPIRepo.SaveChanges();

			return NoContent();
		}
	}
}
