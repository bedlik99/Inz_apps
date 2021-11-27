using ServerAPI.Entities;
using ServerAPI.Repositories;
using ServerAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Services
{
	public class DataBaseSeeder
	{
		private readonly ServerDBContext _dBContext;
		private readonly IEmployeeUserRepo _employeeUserRepo;
		public DataBaseSeeder(ServerDBContext dBContext, IEmployeeUserRepo employeeUserRepo)
		{
			_dBContext = dBContext;
			_employeeUserRepo = employeeUserRepo;
		}
		public void Seed()
		{
			if (_dBContext.Database.CanConnect())
			{
				if (!_dBContext.LaboratoryItems.Any())
				{
					var labs = GetLabs();
					_dBContext.LaboratoryItems.AddRange(labs);
					_dBContext.SaveChanges();
				}

				if (!_dBContext.RegisteredUserItems.Any())
				{
					var users = GetUsers();
					_dBContext.RegisteredUserItems.AddRange(users);
					_dBContext.SaveChanges();
				}
				
				//TBC
				if (!_dBContext.RoleItems.Any())
				{
					var roles = GetRoles();
					_dBContext.RoleItems.AddRange(roles);
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
					LabOrganizer = "Mateusz",
					LaboratoryRequirements = new HashSet<LaboratoryRequirement>()
					{
						new LaboratoryRequirement()
						{
							Content = "",
							ExpirationDate = DateTime.UtcNow.AddDays(14)
						}
					}
				},
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
				new RegisteredUser
				{
					Email = "testingforwebdev@gmail.com",
					UniqueCode = Cryptography.GenerateUniqueCode(),
					EventRegistries = new HashSet<RecordedEvent>()
					{
						new RecordedEvent()
						{
						DateTime = DateTime.UtcNow,
						RegistryContent = "Rejestracja uzytkownika do systemu"
						}
					},
					NoWarning=false,
					Laboratory = _dBContext.LaboratoryItems.Single(x=>x.LabName == "PKC_ONOS")
				},
				new RegisteredUser
				{
					Email = "01143845@pw.edu.pl",
					UniqueCode = Cryptography.GenerateUniqueCode(),
					EventRegistries = new HashSet<RecordedEvent>()
					{
						new RecordedEvent()
						{
						DateTime = DateTime.UtcNow,
						RegistryContent = "Rejestracja uzytkownika do systemu"
						}
					},
					NoWarning=false,
					Laboratory = _dBContext.LaboratoryItems.Single(x=>x.LabName == "PKC_ONOS")
				}
			};
			return users;
		}
	}
}
