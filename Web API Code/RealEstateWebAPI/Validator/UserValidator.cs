using FluentValidation;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Validator
{
	public class UserValidator : AbstractValidator<UserModel>
	{
		public UserValidator()
		{
			RuleFor(user => user.UserName)
				.NotEmpty().WithMessage("Username is required.")
				.Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

			RuleFor(user => user.PhoneNumber)
				.NotEmpty().WithMessage("Phone number is required.")
				.Matches(@"^\d{10}$").WithMessage("Phone number must be a valid 10-digit number.");

			RuleFor(user => user.FirstName)
				.NotEmpty().WithMessage("First name is required.")
				.MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

			RuleFor(user => user.LastName)
				.NotEmpty().WithMessage("Last name is required.")
				.MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

			RuleFor(user => user.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid email format.");

			RuleFor(user => user.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(8).WithMessage("Password must be at least 8 characters.")
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches("[0-9]").WithMessage("Password must contain at least one number.")
				.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

			RuleFor(user => user.UserRole)
				.NotEmpty().WithMessage("User role is required.");

			RuleFor(user => user.Description)
				.MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");


			RuleFor(user => user.Address)
				.MaximumLength(250).WithMessage("Address cannot exceed 250 characters.");
		}
	}
}
