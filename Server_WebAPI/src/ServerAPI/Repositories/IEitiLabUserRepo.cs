using ServerAPI.DTOs;
using ServerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Repositories
{
	public interface IEitiLabUserRepo
	{
		//POST Method - Record Event
		bool ProcessEventContent(MessageDTO encryptedMessage);
		//POST Method - Register User
		bool ProcessUserInitData(MessageDTO encryptedMessage);
	}
}
