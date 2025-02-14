using FluentValidation;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Validator
{
	public class FavoriteValidator : AbstractValidator<AddFavoriteModel>
	{
		public FavoriteValidator()
		{
			RuleFor(favorite => favorite.UserID)
				.NotEmpty().WithMessage("User ID is required.")
				.GreaterThanOrEqualTo(1).WithMessage("User ID must be greater than or equal to 1.")
				.WithName("User ID");
			RuleFor(favorite => favorite.PropertyID)
				.NotEmpty().WithMessage("Property ID is required.")
				.GreaterThanOrEqualTo(1).WithMessage("Property ID must be greater than or equal to 1.")
				.WithName("Property ID");
		}
	}
}
