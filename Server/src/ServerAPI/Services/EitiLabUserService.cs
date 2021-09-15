using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerAPI.DTOs;
using ServerAPI.Exceptions;
using ServerAPI.Models;
using ServerAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServerAPI.Repositories
{
	public class EitiLabUserService : IEitiLabUserRepo
	{
		private readonly ServerDBContext _context;
		private readonly ILogger<EitiLabUserService> _logger;

		public EitiLabUserService(ServerDBContext context, ILogger<EitiLabUserService> logger)
		{
			_context = context;
			_logger = logger;
		}
		public string ProcessUserInitData(MessageDTO encryptedMessage)
		{
			RegisteredUserDTO registeredUser = DecryptRegisterMessage(encryptedMessage.Value);
			if (registeredUser != null)
			{
				if (ValidateUserData(registeredUser))
				{
					RegisteredUser userToSave = new RegisteredUser(registeredUser.IndexNr, registeredUser.UniqueCode);
					_context.RegisteredUserItems.Add(userToSave);
					_context.RecordedEventItems.Add(new RecordedEvent("Maszyna zostala zarejestrowana", DateTime.Now, userToSave));
					_context.SaveChanges();
					return EncryptMessage(registeredUser.IndexNr);
				}
				return null;
			}
			return null;
		}

		public void ProcessEventContent(MessageDTO encryptedMessage)
		{
			RecordedEventDTO recordedEvent = DecryptLogMessage(encryptedMessage.Value);
			RegisteredUser searchedUser;
			
			if(recordedEvent != null)
			{
				searchedUser = GetRegisteredUserIndex(recordedEvent.IndexNr);
				_context.RecordedEventItems.Add(new RecordedEvent(recordedEvent.RegistryContent,DateTime.Now, searchedUser));
				_context.SaveChanges();
			}		
		}

		private RecordedEventDTO DecryptLogMessage(string encryptedMessage)
		{
			RecordedEventDTO recordedEventDTO = null;
			try
			{
				string decryptedMessage = Utility.Cryptography.Decrypt(encryptedMessage);
				decryptedMessage = RemoveHashtagsFromString(decryptedMessage);
				recordedEventDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordedEventDTO>(decryptedMessage);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.StackTrace);
			}
			return recordedEventDTO;
		}

		private string EncryptMessage(string str)
		{
			string encryptedResponse = "";
			str = FillStringWithHashtags(str);
			try
			{
				encryptedResponse = Cryptography.Encrypt(str);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.StackTrace);
			}
			return encryptedResponse;
		}

		private string FillStringWithHashtags(string str)
		{
			if (str.Length == 0)
				return "";

			long stringNecessarySize = FindMaxRangeOfStringLength(str.Length, 0, 16);
			StringBuilder strBuilder = new StringBuilder(str);
			long numberOfNeededHashtags = stringNecessarySize - str.Length;

			for (long i = 0; i < numberOfNeededHashtags; i++)
			{
				strBuilder.Insert(0, "#");
			}
			return strBuilder.ToString();
		}

		private long FindMaxRangeOfStringLength(long strLength, int lowRange, int highRange)
		{
			if (strLength >= lowRange && strLength < highRange)
			{
				return highRange;
			}
			lowRange += 16;
			highRange += 16;
			return FindMaxRangeOfStringLength(strLength, lowRange, highRange);
		}

		private bool ValidateUserData(RegisteredUserDTO registeredUser)
		{
			Regex regex = new Regex("[0-9]{6}");
			return regex.IsMatch(registeredUser.IndexNr) && registeredUser.UniqueCode.Length == 6;
		}

		private RegisteredUserDTO DecryptRegisterMessage(string encryptedMessage)
		{
			RegisteredUserDTO registeredUserDTO = null;
			try
			{
				string decryptedMessage = Cryptography.Decrypt(encryptedMessage);
				decryptedMessage = RemoveHashtagsFromString(decryptedMessage);
				registeredUserDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisteredUserDTO>(decryptedMessage);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.StackTrace);
			}
			return registeredUserDTO;
		}

		private string RemoveHashtagsFromString(string str)
		{
			int strLength = str.Length;
			for (int i = 0; i < strLength; i++)
			{
				if (str[i] != '#')
				{
					str = str.Substring(i);
					break;
				}
			}
			return str;
		}

		public IEnumerable<RegisteredUser> GetAllRegisteredUsers()
		{
			var users = _context
				.RegisteredUserItems
				.Include(u => u.EventRegistries);
			return users;
		}

		public RegisteredUser GetRegisteredUserIndex(string indexNum)
		{
			//_logger.LogError($"User with id: {id}. GET action invoked");

			var user = _context
				.RegisteredUserItems
				.SingleOrDefault(u => u.IndexNum == indexNum);
			if(user is null)
			{
				throw new NotAcceptableException("Not acceptable");
			}		
			return user;
		}
		public RegisteredUser GetRegisteredUser(int id)
		{
			//_logger.LogError($"User with id: {id}. GET action invoked");

			var user = _context
				.RegisteredUserItems
				.Include(u => u.EventRegistries)
				.SingleOrDefault(u => u.Id == id);
			if(user is null)
			{
				throw new NotAcceptableException("Not acceptable");
			}		
			return user;
		}
	}
}
