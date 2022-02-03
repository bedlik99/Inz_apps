using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	/// <summary>
	/// Encja Employee służy do opisu użytkownika zarządzającego laboratorium.
	/// </summary>
	public class Employee
	{
		/// <summary>
		/// Id użytkownika zarządzającego laboratorium.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Nazwa użytkownika
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Hasło użytkownika
		/// </summary>
		public string Password { get; set; }
       
		public int RoleId { get; set; }
        public virtual Role Role { get; set; }
	}
}
