using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	public class RegisteredUser
	{
		/// <summary>
		/// Encja RegisteredUser służy do opisu zarejestrowanego studenta w systemie na konkretne laboratorium.
		/// </summary>
		public RegisteredUser() { }
		public RegisteredUser(string email, string uniqueCode)
		{
			Email = email;
			UniqueCode = uniqueCode;
		}
		public RegisteredUser(string email, string uniqueCode, Laboratory laboratory)
		{
			Email = email;
			UniqueCode = uniqueCode;
			Laboratory = laboratory;
		}

		/// <summary>
		/// Id zarejestrowanego użytkownika 
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Adres email w domenowej serwera poczty elektronicznej pw.edu.pl
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Wygenerowany unikatowy 8 znakowy ciąg służący do poświadczeń studentowi na laboratorium
		/// </summary>
		public string UniqueCode { get; set; }

		/// <summary>
		/// Flaga definiująca spełnienie wymagań laboratoryjnych
		/// </summary>
		public bool NoWarning { get; set; }

		/// <summary>
		/// Definiuje datę spełnienia wszystkich wymagań laboratoryjnych 
		/// </summary>
		public DateTime CompletionDate { get; set; }

		public int LaboratoryId { get; set; }
		public virtual Laboratory Laboratory { get; set; }
		public virtual HashSet<RecordedEvent> EventRegistries { get; set; }
		public virtual HashSet<ExecutedCommand> ExecutedCommands { get; set; }
	}
}

