using FluentValidation;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.DTOs.Validators
{
	public class RegisteredEmployeeDTOValidator : AbstractValidator<RegisteredEmployeeDto>
	{
		public RegisteredEmployeeDTOValidator(ServerDBContext dbContext)
		{
			RuleFor(x => x.Username)
				.NotEmpty();

			RuleFor(x => x.Password)
				.NotEmpty()
				.MinimumLength(6);

			RuleFor(x => x.ConfirmedPassword)
				.Equal(y => y.Password);

			RuleFor(x => x.Username)
				.Custom((value, context) =>
				{
					var emailInUse = dbContext.EmployeeItems.Any(x => x.Username == value);
					if (emailInUse)
					{
						context.AddFailure("Username", "Username already in use");
					}
				});
		}
	}
}
