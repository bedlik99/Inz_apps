using ServerAPI.Models;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Services
{
	public class UserSeeder
	{
		private readonly ServerDBContext _dBContext;

		public UserSeeder(ServerDBContext dBContext)
		{
			_dBContext = dBContext;
		}
		public void Seed()
		{
			if (_dBContext.Database.CanConnect())
			{
				if (!_dBContext.RegisteredUserItems.Any())
				{
					var users = GetUsers();
					_dBContext.RegisteredUserItems.AddRange(users);
					_dBContext.SaveChanges();
				}
			}
			
		}

		private IEnumerable<RegisteredUser> GetUsers()
		{
			var users = new List<RegisteredUser>()
			{
				new RegisteredUser()
				{
					IndexNum = "300100",
					UniqueCode = "ABCDEF",
					EventRegistries = new HashSet<RecordedEvent>()
					{
						new RecordedEvent()
						{
							RegistryContent = "Pomyślnie zarejestrowano użytkownika",
							DateTime = DateTime.UtcNow
						}
					}
				},

				new RegisteredUser()
				{
					IndexNum = "300200",
					UniqueCode = "GHIJKL",
					EventRegistries = new HashSet<RecordedEvent>()
					{
						new RecordedEvent()
						{
							RegistryContent = $"Pomyślnie zarejestrowano użytkownika",
							DateTime = DateTime.UtcNow
						}
					}
				},

				new RegisteredUser()
				{
					IndexNum = "300300",
					UniqueCode = "MNOPRS",
					EventRegistries = new HashSet<RecordedEvent>()
					{
						new RecordedEvent()
						{
							RegistryContent = $"Pomyślnie zarejestrowano użytkownika",
							DateTime = DateTime.UtcNow
						}
					}
				}
			};
			return users;
		}
	}
}
