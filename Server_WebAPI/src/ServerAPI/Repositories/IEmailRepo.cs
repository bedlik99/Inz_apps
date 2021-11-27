using ServerAPI.DTOs;
using ServerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Repositories
{
	public interface IEmailRepo
	{
		string SendEmail(EmailData emailData);
		string SendEmail(IEnumerable<RegisteredLabUserDTO> emailData, string labName);
		string SendEmailDbData(RegisteredUser user);
	}
}
