using FluentValidation;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Validator
{
	public class PropertyValidator : AbstractValidator<PropertyModel>
	{
		public PropertyValidator()
		{
			RuleFor(property => property.UserID)
				.GreaterThan(0).WithMessage("UserID must be greater than 0.")
				.WithName("User ID");

			RuleFor(property => property.PropertyTitle)
				.NotEmpty().WithMessage("Property Title is required.")
				.MaximumLength(150).WithMessage("Property Title cannot exceed 150 characters.")
				.WithName("Property Title");

			RuleFor(property => property.PropertyDescription)
				.NotEmpty().WithMessage("Property Description is required.")
				.MaximumLength(1000).WithMessage("Property Description cannot exceed 1000 characters.")
				.WithName("Property Description");

			RuleFor(property => property.PropertyPrice)
				.GreaterThan(0).WithMessage("Property Price must be greater than 0.")
				.WithName("Property Price");

			RuleFor(property => property.PropertyAddress)
				.NotEmpty().WithMessage("Property Address is required.")
				.MaximumLength(250).WithMessage("Property Address cannot exceed 250 characters.")
				.WithName("Property Address");

			RuleFor(property => property.PropertySize)
				.GreaterThan(0).WithMessage("Property Size must be greater than 0.")
				.WithName("Property Size");

			RuleFor(property => property.BedroomCount)
				.GreaterThan(0).WithMessage("Bedroom Count must be greater than 0.")
				.WithName("Bedroom Count");

			RuleFor(property => property.BathroomCount)
				.GreaterThan(0).WithMessage("Bathroom Count must be greater than 0.")
				.WithName("Bathroom Count");

			RuleFor(property => property.BuildYear)
				.LessThanOrEqualTo(DateTime.Now).WithMessage("Build Date cannot be in the future.")
				.GreaterThanOrEqualTo(DateTime.Parse("1900-01-01")).WithMessage("Build Date must be a valid date after 01-01-1900.")
				.WithName("Build Date");

			RuleFor(property => property.PropertyType)
				.NotEmpty().WithMessage("Property Type is required.")
				.WithName("Property Type");
			RuleFor(property => property.TransactionType)
				.NotEmpty().WithMessage("Transaction Type is required.")
				.WithName("Transaction Type");

			RuleFor(property => property.ParkingSpaces)
				.GreaterThanOrEqualTo(0).WithMessage("Parking Spaces cannot be negative.")
				.WithName("Parking Spaces");

			RuleFor(property => property.Images).NotEmpty().WithMessage("Property images required.");
		}
	}
}
