using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class AppointmentController : ControllerBase
	{
		private readonly AppointmentRepository _appointmentRepository;

		public AppointmentController(AppointmentRepository appointmentRepository)
		{
			_appointmentRepository = appointmentRepository;
		}

		#region Schedule Appointment
		[HttpPost]
		public IActionResult ScheduleAppointment([FromBody] AppointmentModel appointment)
		{

			if (appointment == null)
			{
				return BadRequest(new { Message = "Invalid appointment data provided." });
			}

			if (appointment.AppointmentUserID == null || appointment.PropertyID == null)
			{
				return BadRequest(new { Message = "Appointment User and Property must be provided." });
			}

			bool isScheduled = _appointmentRepository.ScheduleAppointment(appointment);
			if (isScheduled)
			{
				return Ok(new { Message = "Appointment scheduled successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while scheduling the appointment." });
		}
		#endregion

		#region Update Appointment Status
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpPut("{id}")]
		public IActionResult UpdateAppointmentStatus(int id, [FromBody] AppointmentStatus status)
		{
			if (string.IsNullOrWhiteSpace(status.Status))
			{
				return BadRequest(new { Message = "Invalid status provided." });
			}

			bool isUpdated = _appointmentRepository.UpdateAppointmentStatus(id, status);
			if (isUpdated)
			{
				return Ok(new { Message = "Appointment status updated successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while updating the appointment status." });
		}
		#endregion

		#region Get Appointments by User ID
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpGet("{userId}")]
		public IActionResult GetAppointmentsByUser(int userId)
		{
			var appointments = _appointmentRepository.GetAppointmentsByUser(userId);
			if (!appointments.Any())
			{
				return NotFound(new { Message = "No appointments found for the specified user." });
			}
			return Ok(appointments);
		}
		#endregion

		#region Get Appointments by Property ID
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpGet("property/{propertyId}")]
		public IActionResult GetAppointmentsByProperty(int propertyId)
		{
			var appointments = _appointmentRepository.GetAppointmentsByProperty(propertyId);
			if (!appointments.Any())
			{
				return NotFound(new { Message = "No appointments found for the specified property." });
			}
			return Ok(appointments);
		}
		#endregion

		#region Get Appointments by Status
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpGet("status/{status}")]
		public IActionResult GetAppointmentsByStatus(string status)
		{
			var appointments = _appointmentRepository.GetAppointmentsByStatus(status);
			if (!appointments.Any())
			{
				return NotFound(new { Message = "No appointments found with the specified status." });
			}
			return Ok(appointments);
		}
		#endregion

		#region User Drop Down
		[HttpGet]
		public IActionResult GetUserDropDown()
		{
			var users = _appointmentRepository.UserDropDown();
			return Ok(users);
		}
		#endregion

		#region Property Drop Down
		[HttpGet("{userId}")]
		public IActionResult GetPropertyDropDown(int userId)
		{
			var properties = _appointmentRepository.PropertyDropDown(userId);
			return Ok(properties);
		}
		#endregion
	}
}
