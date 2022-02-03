using Microsoft.EntityFrameworkCore;
using Moq;
using ServerAPI.Entities;
using ServerAPI.Repositories;
using ServerAPI.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_WebAPI.Tests.Common
{
    public static class ServerDBContextFactory
    {
        public static Mock<ServerDBContext> Create()
        {
            var options = new DbContextOptionsBuilder<ServerDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var mock = new Mock<ServerDBContext>(options) { CallBase = true };

            var context = mock.Object;

            context.Database.EnsureCreated();


            var lab = new Laboratory()
            {
                Id = 1,
                LabName = "PKC",
                LabOrganizer = "Organizer",
                LaboratoryRequirements = new HashSet<LaboratoryRequirement>()
                {
                    new LaboratoryRequirement()
                    {
                        Content = "ls"
                    }
                }
            };
            context.LaboratoryItems.Add(lab);

            var user = new RegisteredUser
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
                NoWarning = false,
                Laboratory = lab,
                ExecutedCommands = new HashSet<ExecutedCommand>()
            };
            var user2 = new RegisteredUser
            {
                Email = "88888888@pw.edu.pl",
                UniqueCode = Cryptography.GenerateUniqueCode(),
                EventRegistries = new HashSet<RecordedEvent>()
                {
                    new RecordedEvent()
                    {
                        DateTime = DateTime.UtcNow,
                        RegistryContent = "Rejestracja uzytkownika do systemu"
                    }
                },
                NoWarning = false,
                Laboratory = lab,
                ExecutedCommands = new HashSet<ExecutedCommand>()
            };
            context.RegisteredUserItems.Add(user);
            context.RegisteredUserItems.Add(user2);

            var role = new Role() { Id = 3, RoleName = "User" };
            context.RoleItems.Add(role);

            var employee = new Employee
            {
                Id = 1,
                Username = "Mateusz",
                Password = "haslo",
                RoleId = 3
            };
            context.EmployeeItems.Add(employee);

            context.SaveChanges();

            return mock;
        }

        public static void Destroy(ServerDBContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
