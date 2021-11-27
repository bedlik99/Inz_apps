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
			RecordedEventDTO recordedEvent = (RecordedEventDTO)DecryptMessage(encryptedMessage.Value, false);
			if (recordedEvent != null  && !string.IsNullOrEmpty(recordedEvent.UniqueCode) )
			{
				var executedCommand = GetExecutedCommand(recordedEvent);
				
				if(executedCommand!=null) executedCommand = executedCommand.Trim();

				RegisteredUser searchedUser = FindStudentByUniqueCode(recordedEvent.UniqueCode);
				if (searchedUser != null)
				{
					_context.RecordedEventItems.Add(new RecordedEvent(recordedEvent.RegistryContent, DateTime.Now, searchedUser));
					if (executedCommand != null && searchedUser.NoWarning==false)
					{
						AddExecutedCommand(searchedUser, executedCommand);
						CheckIfCompletedRequirements(searchedUser);
					}
					_context.SaveChanges();
					return true;
				}
				return false;
			}
			return false;
		}

		private void CheckIfCompletedRequirements(RegisteredUser searchedUser)
		{
			var areRequirementsDone = searchedUser
				.Laboratory
				.LaboratoryRequirements
				.All(x => searchedUser
				.ExecutedCommands.Any(y => y.Content == x.Content));

			if (areRequirementsDone) 
			{
				searchedUser.NoWarning = true;
				searchedUser.CompletionDate = DateTime.Now;
				_context.RegisteredUserItems.Update(searchedUser);
				_context.SaveChanges();
			}
		}

		private void AddExecutedCommand(RegisteredUser searchedUser, string executedCommand)
		{
			if (searchedUser.ExecutedCommands is null)
			{
				searchedUser.ExecutedCommands = new HashSet<ExecutedCommand>();
			}
			if (_context.LaboratoryRequirementsItems.Any(x => x.Content == executedCommand)
			&& !searchedUser.ExecutedCommands.Any(x => x.Content == executedCommand))
			{
				searchedUser.ExecutedCommands.Add(new ExecutedCommand(executedCommand, searchedUser, DateTime.Now));
				_context.RegisteredUserItems.Update(searchedUser);
				_context.SaveChanges();
			}
		}

		private string GetExecutedCommand(RecordedEventDTO recordedEvent)
		{
			if (recordedEvent.RegistryContent.Length > 12 && recordedEvent.RegistryContent.Substring(0, 13) == "bash_command>")
				return recordedEvent.RegistryContent.Substring(13);
			return null;
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

		public RegisteredUser FindStudentByUniqueCode(string uniqueCode)
		{
			var user = _context
				.RegisteredUserItems
				.Include(u => u.EventRegistries)
				.Include(l => l.Laboratory)
				.Include(l=>l.Laboratory.LaboratoryRequirements)
				.Include(r => r.ExecutedCommands)
				.SingleOrDefault(u => u.UniqueCode == uniqueCode);
			if (user is null)
			{
				throw new NotAcceptableException("No such student with given unique code");
			}
			return user;
		}
	}
}