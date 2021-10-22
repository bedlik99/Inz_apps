using ServerAPI.Entities;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Services
{
	public class DataBaseSeeder
	{
		private readonly ServerDBContext _dBContext;

		public DataBaseSeeder(ServerDBContext dBContext)
		{
			_dBContext = dBContext;
		}
		public void Seed()
		{
			if (_dBContext.Database.CanConnect())
			{
				if (!_dBContext.RoleItems.Any())
				{
					var roles = GetRoles();
					_dBContext.RoleItems.AddRange(roles);
					_dBContext.SaveChanges();
				}

				if (!_dBContext.RegisteredUserItems.Any())
				{
					var users = GetUsers();
					_dBContext.RegisteredUserItems.AddRange(users);
					_dBContext.SaveChanges();
				}

				if (!_dBContext.LaboratoryItems.Any())
				{
					var labs = GetLabs();
					_dBContext.LaboratoryItems.AddRange(labs);
					_dBContext.SaveChanges();
				}
			}
		}

		private IEnumerable<Laboratory> GetLabs()
		{
			var labs = new List<Laboratory>()
			{
				new Laboratory()
				{
					LabName = "PKC_ONOS",
					LabOrganizer = "Jerzy"
				},
				new Laboratory()
				{
					LabName = "PKC_REST",
					LabOrganizer = "Stanislaw"
				}
			};

			return labs;
		}

		private IEnumerable<Role> GetRoles()
		{
			var roles = new List<Role>()
			{
				new Role()
				{
					RoleName = "Administrator"
				},
				new Role()
				{
					RoleName = "Employee"
				}
			};
			return roles;
		}

		private IEnumerable<RegisteredUser> GetUsers()
		{
			var users = new List<RegisteredUser>()
			{
				new RegisteredUser()
				{
					Email = "jozek.maly.stud@pw.edu.pl",
					UniqueCode = "ABCDEF55",
					EventRegistries = new HashSet<RecordedEvent>()
					{
						new RecordedEvent()
						{
							RegistryContent = "Pomyślnie zarejestrowano użytkownika",
							DateTime = DateTime.UtcNow
						}
					},
					Laboratory = _dBContext.LaboratoryItems.Single(x=>x.Id == 3)
				},

				new RegisteredUser()
				{
					Email = "arkadiusz.maly.stud@pw.edu.pl",
					UniqueCode = "GHIJK34L",
					EventRegistries = new HashSet<RecordedEvent>()
					{
						new RecordedEvent()
						{
							RegistryContent = $"Pomyślnie zarejestrowano użytkownika",
							DateTime = DateTime.UtcNow
						}
					},
					Laboratory = _dBContext.LaboratoryItems.Single(x=>x.Id == 3)
				},

				new RegisteredUser()
				{
					Email = "michal.kozuch.stud@pw.edu.pl",
					UniqueCode = "MNOPRS12",
					EventRegistries = new HashSet<RecordedEvent>()
					{
						new RecordedEvent()
						{
							RegistryContent = $"Pomyślnie zarejestrowano użytkownika",
							DateTime = DateTime.UtcNow
						}
					},
					Laboratory = _dBContext.LaboratoryItems.Single(x=>x.Id == 3)	
				}
			};
			return users;
		}
	}
}
