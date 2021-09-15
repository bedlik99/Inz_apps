using ServerAPI.DTOs;
using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Repositories
{
	public interface IEitiLabUserRepo
	{
		//POST Method - Record Event
		void ProcessEventContent(MessageDTO encryptedMessage);
		//POST Method - Register User
		string ProcessUserInitData(MessageDTO encryptedMessage);
		//GET Method - All Users
		IEnumerable<RegisteredUser> GetAllRegisteredUsers();
		//GET Method - Specific User
		RegisteredUser GetRegisteredUser(int id);
		//void FindStudentByIndexNum(string indexNum);
	}
}
