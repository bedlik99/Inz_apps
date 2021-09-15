using AutoMapper;
using StudentAPI.DTOs;
using StudentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Profiles
{
	public class StudentsProfile : Profile
	{
		public StudentsProfile()
		{
			//Mapping source object (Studnet) to target (Dto)
			CreateMap<Student, StudentReadDto>();
			CreateMap<StudentCreateDto, Student>();
			CreateMap<StudentUpdateDto, Student>();
			CreateMap<Student, StudentUpdateDto>();
		}
	}
}
