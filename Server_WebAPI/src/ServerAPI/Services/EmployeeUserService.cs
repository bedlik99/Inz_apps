using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.DTOs;
using ServerAPI.Entities;
using ServerAPI.Exceptions;
using ServerAPI.Profiles;
using ServerAPI.Repositories;
using ServerAPI.Settings;
using ServerAPI.Utility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI.Services
{
    public class EmployeeUserService : IEmployeeUserRepo
    {
        private readonly ServerDBContext _context;
        private readonly IPasswordHasher<Employee> _hasher;
        private readonly AuthenticationSettings _authenticationSettings;
        public EmployeeUserService(ServerDBContext context)
        {
            _context = context;
        }
        public EmployeeUserService(ServerDBContext context, IPasswordHasher<Employee> hasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _hasher = hasher;
            _authenticationSettings = authenticationSettings;
        }

        public string GenerateJwt(LoginDto loginDto)
        {
            var employee = _context.EmployeeItems
                .Include(e => e.Role)
                .FirstOrDefault(u => u.Username == loginDto.Username);

            if (employee is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _hasher.VerifyHashedPassword(employee, employee.Password, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,employee.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{employee.Username}"),
                new Claim(ClaimTypes.Role, $"{employee.Role.RoleName}")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public void RegisterEmployee(RegisteredEmployeeDto registeredEmployeeDto)
        {
            Employee employee = new Employee
            {
                Username = registeredEmployeeDto.Username,
            };
            var hashedPassword = _hasher.HashPassword(employee, registeredEmployeeDto.Password);
            employee.Password = hashedPassword;
            _context.EmployeeItems.Add(employee);
            _context.SaveChanges();
        }

        public RegisteredUser GetRegisteredUser(string email, string labName)
        {
            var user = _context
                .RegisteredUserItems
                .Include(u => u.EventRegistries)
                .Include(l => l.Laboratory)
                .Include(r => r.Laboratory.LaboratoryRequirements)
                .Include(c => c.ExecutedCommands)
                .Where(l => l.Laboratory.LabName == labName)
                .FirstOrDefault(u => u.Email == email);

            if (user is null)
            {
                throw new NotFoundException("Not acceptable");
            }
            return user;
        }

        public IEnumerable<RecordedEventDTO> GetUserRegistries(string email, string labName)
        {
            List<RecordedEventDTO> recordedEventsList = new List<RecordedEventDTO>();
            RegisteredUser searchedUser = GetRegisteredUser(email, labName);
            if (email != null && searchedUser != null)
            {
                HashSet<RecordedEvent> recordedEvents = searchedUser.EventRegistries;
                recordedEvents = recordedEvents.OrderBy(c => c.DateTime).ToHashSet();
                recordedEvents.ToList().ForEach(rec => recordedEventsList.Add(new RecordedEventDTO(rec.RegistryContent)));
            }
            return recordedEventsList;
        }

        public IEnumerable<RegisteredUser> GetAllRegisteredUsers()
        {
            var users = _context
                .RegisteredUserItems
                .Include(u => u.EventRegistries)
                .Include(l => l.Laboratory)
                .Include(r => r.Laboratory.LaboratoryRequirements)
                .Include(c => c.ExecutedCommands)
                .ToList();
            if (!users.Any())
                return null;
            return users;
        }

        public IEnumerable<RegisteredUser> GetUsersByLab(string labName)
        {
            var users = _context
                .RegisteredUserItems
                .Include(u => u.EventRegistries)
                .Include(l => l.Laboratory)
                .Include(r => r.Laboratory.LaboratoryRequirements)
                .Include(c => c.ExecutedCommands)
                .Where(l => l.Laboratory.LabName == labName)
                .ToList();
            if (!users.Any())
                return null;
            return users;
        }
        public bool DeleteRegisteredUser(string email, string labName)
        {
            var user = _context
                .RegisteredUserItems
                .Where(l => l.Laboratory.LabName == labName)
                .FirstOrDefault(u => u.Email == email);
            if (user is null)
            {
                return false;
            }
            _context.Remove(user);
            _context.SaveChanges();
            return true;
        }
        public IEnumerable<Employee> GetAllRegisteredEmployees()
        {
            var employees = _context
                .EmployeeItems
                .Include(r => r.Role)
                .ToList();
            if (!employees.Any())
                return null;
            return employees;
        }
        public IEnumerable<RegisteredUser> UploadFileService(IFormFile fileUpload, string lab, string owner)
        {
            using (var stream = fileUpload.OpenReadStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var reader = new CsvReader(streamReader, System.Globalization.CultureInfo.CurrentCulture);
                    reader.Context.RegisterClassMap<CSVProfileUser>();
                    var users = reader.GetRecords<RegisteredUser>().ToList();
                    if (!_context.LaboratoryItems.Any(x => x.LabName == lab))
                    {
                        _context.LaboratoryItems.Add(new Laboratory
                        {
                            LabName = lab,
                            LabOrganizer = owner,
                            LaboratoryRequirements = new HashSet<LaboratoryRequirement>() { }
                        });
                        _context.SaveChanges();
                    }
                    foreach (var user in users)
                    {
                        user.Laboratory = _context.LaboratoryItems.SingleOrDefault(l => l.LabName == lab);
                    }
                    return users;
                }
            }
        }
        public IEnumerable<RegisteredLabUserDTO> InsertUsersIntoDataBase(IEnumerable<RegisteredUser> uploadResult)
        {
            HashSet<RegisteredLabUserDTO> set = new HashSet<RegisteredLabUserDTO>();
            foreach (var user in uploadResult)
            {
                if (user != null)
                {
                    if (_context.RegisteredUserItems.Any(x => x.Email == user.Email && x.Laboratory.LabName == user.Laboratory.LabName))
                    {
                        continue;
                    }
                    user.UniqueCode = Cryptography.GenerateUniqueCode();
                    user.UniqueCode = CheckUniquenessOfCode(user);

                    _context.RegisteredUserItems.Add(user);
                    _context.RecordedEventItems.Add(new RecordedEvent("Uzytkownik zarejestrowany", DateTime.Now, user));
                    _context.SaveChanges();
                    set.Add(new RegisteredLabUserDTO(user.UniqueCode, user.Email));
                }
            }
            return set;
        }

        public string CheckUniquenessOfCode(RegisteredUser user)
        {
            if (_context.RegisteredUserItems.Any(x => x.UniqueCode == user.UniqueCode))
            {
                user.UniqueCode = Cryptography.GenerateUniqueCode();
                return CheckUniquenessOfCode(user);
            }
            return user.UniqueCode;
        }

        public bool UploadLabRequirements(IFormFile fileUpload, string lab)
        {
            var anyChanges = false;
            using (var stream = fileUpload.OpenReadStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var reader = new CsvReader(streamReader, System.Globalization.CultureInfo.CurrentCulture);
                    reader.Context.RegisterClassMap<CSVProfileLaboratoryRequirements>();
                    var requirements = reader.GetRecords<LaboratoryRequirement>().ToList();
                    if (!_context.LaboratoryItems.Any(x => x.LabName == lab))
                    {
                        throw new NotFoundException("Takie laboratorium nie istenieje.");
                    }
                    var laboratory = _context.LaboratoryItems.SingleOrDefault(x => x.LabName == lab);
                    foreach (var requirement in requirements)
                    {
                        if (!_context.LaboratoryRequirementsItems.Any(x => x.Content == requirement.Content))
                        {
                            _context.LaboratoryRequirementsItems.Add(new LaboratoryRequirement(requirement.Content, laboratory));

                            _context.SaveChanges();
                            anyChanges = true;
                        }
                    }
                    return anyChanges;
                }
            }
        }

        public StringBuilder GenerateResults(string labName)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Email;LabName;NoWarning;CompletionDate;");
            var dataBaseResult = _context
                .RegisteredUserItems
                .Where(x => x.Laboratory.LabName == labName)
                .Select(x => new RegisteredUser
                {
                    Email = x.Email,
                    Laboratory = x.Laboratory,
                    NoWarning = x.NoWarning,
                    CompletionDate = x.CompletionDate
                });
            if (!dataBaseResult.Any())
                return null;
            foreach (var user in dataBaseResult)
            {
                builder.AppendLine($"{user.Email};{user.Laboratory.LabName};{user.NoWarning};{user.CompletionDate}");
            }
            return builder;
        }

        public StringBuilder GenerateResult(string labName, string email)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Email;LabName;NoWarning;CompletionDate;");
            var dataBaseUser = _context
                .RegisteredUserItems
                .Where(x => x.Laboratory.LabName == labName)
                .Select(x => new RegisteredUser
                {
                    Email = x.Email,
                    Laboratory = x.Laboratory,
                    NoWarning = x.NoWarning,
                    CompletionDate = x.CompletionDate
                }).SingleOrDefault(u => u.Email == email);
            if (dataBaseUser == null)
                return null;
            builder.AppendLine($"{dataBaseUser.Email};{dataBaseUser.Laboratory.LabName};{dataBaseUser.NoWarning};{dataBaseUser.CompletionDate};");
            return builder;
        }

        public bool DeleteLaboratoryAndUsersData(string labName)
        {
            var listToRemove = GetUsersByLab(labName);

            var lab = _context.LaboratoryItems.SingleOrDefault(x => x.LabName == labName);
            if (lab != null)
            {
                foreach (var user in listToRemove)
                {
                    _context.RegisteredUserItems.Remove(user);
                    _context.SaveChanges();
                }
                _context.LaboratoryItems.Remove(lab);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<LaboratoryRequirement> GetLabRequirements(string labName)
        {
            var labRequirements = _context
                .LaboratoryRequirementsItems
                .Where(l => l.Laboratory.LabName == labName)
                .ToList();

            if (!labRequirements.Any())
                return null;
            return labRequirements;
        }
    }
}
