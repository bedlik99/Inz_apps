using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerAPI.DTOs;
using ServerAPI.Entities;
using ServerAPI.Exceptions;
using ServerAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
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

		public bool ProcessUserInitData(MessageDTO encryptedMessage)
		{
			RegisteredLabUserDTO registeredUser = (RegisteredLabUserDTO)DecryptMessage(encryptedMessage.Value, true);

			if (registeredUser == null)
			{
				throw new NotAcceptableException("ERROR");
			}
			var userAfterValidation = ValidateUserData(registeredUser);
			if (userAfterValidation != null && registeredUser != null)
			{
				if (_context.RecordedEventItems
					.Any(u => u.RegisteredUserId == userAfterValidation.Id
					&& u.RegistryContent == "Maszyna zostala zarejestrowana"))
				{
					_context.RecordedEventItems.Add(new RecordedEvent("Ponowne logowanie na maszyne", DateTime.Now, userAfterValidation));
					_context.SaveChanges();
				}
				else
				{
					_context.RecordedEventItems.Add(new RecordedEvent("Maszyna zostala zarejestrowana", DateTime.Now, userAfterValidation));
					_context.SaveChanges();
				}
				return true;
			}
			return false;
		}

		public bool ProcessEventContent(MessageDTO encryptedMessage)
		{
			// JA TU POTRZEBUJE TEZ UNIQUE CODE lub labkę. Bez tego nie rozoznie ktory student dodaje logi.
			RecordedEventDTO recordedEvent = (RecordedEventDTO)DecryptMessage(encryptedMessage.Value, false);
			if (recordedEvent != null && !string.IsNullOrEmpty(recordedEvent.Email))
			{
				RegisteredUser searchedUser = FindStudentByEmail(recordedEvent.Email);
				if (searchedUser != null)
				{
					_context.RecordedEventItems.Add(new RecordedEvent(recordedEvent.RegistryContent, DateTime.Now, searchedUser));
					_context.SaveChanges();
					return true;
				}
				return false;
			}
			return false;
		}

		private object DecryptMessage(string encryptedMessage, bool isRegistartion)
		{
			try
			{
				string decryptedMessage = Cryptography.Decrypt(encryptedMessage);
				decryptedMessage = RemoveCharsFromString(decryptedMessage);
				if (isRegistartion)
				{
					return Newtonsoft.Json.JsonConvert.DeserializeObject<RegisteredLabUserDTO>(decryptedMessage);
				}
				else
				{
					return Newtonsoft.Json.JsonConvert.DeserializeObject<RecordedEventDTO>(decryptedMessage);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.WriteLine(e.StackTrace);
			}
			return null;
		}

		private string RemoveCharsFromString(string str)
		{
			if (str[0] != '{' && str[0] != '[')
			{
				string firstASCIIChar = str.Substring(0, 1);
				int decimalValue = Cryptography.ConvertASCIITo10System(firstASCIIChar);
				str = str.Substring(decimalValue);
			}
			return str;
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

		private RegisteredUser ValidateUserData(RegisteredLabUserDTO registeredUser)
		{
			Regex regex = new("[0-9]{8}");
			var check = regex.IsMatch(registeredUser.Email) && registeredUser.UniqueCode.Length == 8;

			if (check)
			{
				var userAuthenticated = _context
				.RegisteredUserItems
				.Include(u => u.EventRegistries)
				.Include(l => l.Laboratory)
				.Include(r => r.Laboratory.LaboratoryRequirements)
				.SingleOrDefault(u => u.Email == registeredUser.Email && u.UniqueCode == registeredUser.UniqueCode);
				return userAuthenticated;
			}
			return null;
		}
		private string EncryptMessage(string str)
		{
			string encryptedResponse = "";
			str = FillStringWithChars(str);
			try
			{
				encryptedResponse = Cryptography.Encrypt(str);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.WriteLine(e.StackTrace);
			}
			return encryptedResponse;
		}

		private string FillStringWithChars(string str)
		{
			if (str.Length == 0)
				return "";

			long stringNecessarySize = str.Length;

			if (stringNecessarySize % 16 != 0)
				stringNecessarySize = FindMaxRangeOfStringLength(str.Length, 0, 16);

			long numberOfNeededChars = stringNecessarySize - str.Length;

			StringBuilder neededChars = new StringBuilder(Cryptography.ConvertASCIITo16System((int)numberOfNeededChars));
			Random random = new Random();

			for (long i = 0; i < numberOfNeededChars - 1; i++)
			{
				int randomCharEmail = (int)Math.Floor(random.NextDouble() * 88);
				neededChars.Append(Cryptography.GetWritableChars()[randomCharEmail]);
			}

			if (numberOfNeededChars != 0)
				str = neededChars + str;

			return str;
		}

		public RegisteredUser FindStudentByEmail(string email)
		{
			//_logger.LogError($"User with id: {id}. GET action invoked");
			var user = _context
				.RegisteredUserItems
				.Include(u => u.EventRegistries)
				.SingleOrDefault(u => u.Email == email);

			if (user is null)
			{
				throw new NotAcceptableException("No such student with given email address");
			}
			return user;
		}
	}
}