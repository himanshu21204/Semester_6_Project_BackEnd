using FluentValidation;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Validator
{
	public class ContactUsValidator : AbstractValidator<ContactUsModel>
	{
		public ContactUsValidator()
		{
			RuleFor(contact => contact.Name)
				.NotEmpty().WithMessage("Name is required.")
				.MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
				.WithName("Full Name");

			RuleFor(contact => contact.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid email format.")
				.WithName("Email Address");

			RuleFor(contact => contact.PhoneNumber)
				.NotEmpty().WithMessage("Phone number is required.")
				.Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits.")
				.WithName("Phone Number");

			RuleFor(contact => contact.Subject)
				.MaximumLength(150).WithMessage("Subject cannot exceed 150 characters.")
				.When(contact => contact.Subject != null)
				.WithName("Subject");

			RuleFor(contact => contact.Message)
				.NotEmpty().WithMessage("Message is required.")
				.MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters.")
				.WithName("Message");
			
		}
	}
}
