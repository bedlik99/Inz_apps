using Microsoft.AspNetCore.Http;
using ServerAPI.DTOs;
using ServerAPI.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI.Repositories
{
	public interface IEmployeeUserRepo
	{
		void RegisterEmployee(RegisteredEmployeeDto registeredEmployeeDto);
		string GenerateJwt(LoginDto loginDto);
		IEnumerable<RegisteredUser> GetAllRegisteredUsers();
		IEnumerable<RegisteredUser> GetUsersByLab(string labName);
		RegisteredUser GetRegisteredUser(string emailAddress);
		IEnumerable<RecordedEventDTO> GetUserRegistries(string emailAddress);
		bool DeleteRegisteredUser(string emailAddress);
		IEnumerable<Employee> GetAllRegisteredEmployees();
		IEnumerable<RegisteredUser> UploadFileService(IFormFile fileUpload,string lab,string owner);
		IEnumerable<RegisteredLabUserDTO> InsertUsersIntoDataBase(IEnumerable<RegisteredUser> uploadResult);
		bool UploadLabRequirements(IFormFile fileUpload,string lab);
		
		// TBD
		bool RemoveLaboratoryAndUsersData();
		StringBuilder GenerateResults(string labName);
		
	}
}
