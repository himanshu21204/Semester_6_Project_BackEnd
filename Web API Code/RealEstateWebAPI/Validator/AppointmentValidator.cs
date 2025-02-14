using FluentValidation;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Validator
{
	public class AppointmentValidator : AbstractValidator<AppointmentModel>
	{
		public AppointmentValidator()
		{
			RuleFor(appointment => appointment.BookerUserID)
				.GreaterThan(0).WithName("Booker Name").WithMessage("Booker Name must be greater than 0.");

			RuleFor(appointment => appointment.AppointmentUserID)
				.NotNull().WithMessage("Seller Name Cannot be null")
				.GreaterThan(0).WithName("Seller Name").WithMessage("Seller Name required.");

			RuleFor(appointment => appointment.PropertyID)
				.NotNull().WithMessage("Property title Cannot be null")
				.GreaterThan(0).WithMessage("Property title required.");

			RuleFor(appointment => appointment.AppointmentStartDate).
				NotNull().WithMessage("Appointment Start Date is required.")
				.NotEmpty().WithMessage("Appointment Start Date is required.")
				.GreaterThanOrEqualTo(DateTime.Now).WithMessage("Appointment Start Date cannot be in the past.");

			RuleFor(appointment => appointment.AppointmentEndDate)
				.NotNull().WithMessage("Appointment End Date is required.")
				.NotEmpty().WithMessage("Appointment End Date is required.")
				.GreaterThan(appointment => appointment.AppointmentStartDate)
				.WithMessage("Appointment End Date must be after Appointment Start Date.");

			RuleFor(appointment => appointment.Status)
				.NotEmpty().WithMessage("Status is required.")
				.Must(status => new[] { "Scheduled", "Completed", "Cancelled", "Rescheduled", "Pending" }.Contains(status))
				.WithMessage("Status must be one of the following: Scheduled, Completed, Cancelled.");

			RuleFor(appointment => appointment.Notes)
				.MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
		}
	}
}
