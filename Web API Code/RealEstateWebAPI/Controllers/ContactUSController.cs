using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class ContactUSController : ControllerBase
	{
		private readonly ContactUSRepository _contactUsRepository;

		public ContactUSController(ContactUSRepository contactUsRepository)
		{
			_contactUsRepository = contactUsRepository;
		}

		#region Insert ContactUs
		[HttpPost]
		public IActionResult InsertContactUs([FromBody] ContactUsModel contactUs)
		{
			if (contactUs == null)
			{
				return BadRequest(new { Message = "Invalid contact us data provided." });
			}

			bool isInserted = _contactUsRepository.InsertContactUs(contactUs);
			if (isInserted)
			{
				return Ok(new { Message = "Contact us entry added successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while adding the contact us entry." });
		}
		#endregion

		#region Get All ContactUs Entries
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult GetAllContactUs()
		{
			var contactUsList = _contactUsRepository.GetAllContactUs();
			if (contactUsList == null || !contactUsList.Any())
			{
				return NotFound(new { Message = "No contact us entries found." });
			}
			return Ok(contactUsList);
		}
		#endregion

		#region Get ContactUs Entry By ID
		[Authorize(Roles = "Admin")]
		[HttpGet("{id}")]
		public IActionResult GetContactUsById(int id)
		{
			var contactUs = _contactUsRepository.GetContactUsById(id);
			if (contactUs == null)
			{
				return NotFound(new { Message = "Contact us entry not found." });
			}
			return Ok(contactUs);
		}
		#endregion
		#region Update Contact US Status
		[Authorize(Roles = "Admin")]
		[HttpPut("{contactID}")]
		public IActionResult UpdateContactUsStatus(int contactID, [FromBody] StatusModel Status)
			{
			if (contactID == 0 || Status.Status.Length < 1)
			{
				return BadRequest(new { Message = "Invalid Data" });
			}

			bool isUpdated = _contactUsRepository.UpdateContactUsStatus(contactID,Status);
			if (isUpdated)
			{
				return Ok(new { Message = "Status updated successfully" });
			}
			return StatusCode(500, new { Message = "An error occurred while updating the Status" });
		}
		#endregion
	}
}
