using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	/// <summary>
	/// Encja Role służy do opisu roli użytkownika zarządzającego laboratorium.
	/// </summary>
	public class Role
	{
		/// <summary>
		/// Id roli
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Nazwa roli użytkownika
		/// </summary>
		public string RoleName { get; set; }
	}
}
