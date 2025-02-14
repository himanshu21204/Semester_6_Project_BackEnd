using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	[EnableCors("AllowMyOrigin")]
	public class PropertyController : ControllerBase
	{
		private readonly PropertyRepository _propertyRepository;

		public PropertyController(PropertyRepository propertyRepository)
		{
			_propertyRepository = propertyRepository;
		}

		[HttpGet]
		public IActionResult GetAllProperties()
		{
			var properties = _propertyRepository.SelectAll();
			return Ok(properties);
		}

		#region Get Property By ID
		[HttpGet("{id}")]
		public IActionResult GetPropertyByID(int id)
		{
			var property = _propertyRepository.SelectByID(id);
			if (property == null)
			{
				return NotFound(new { Message = "Property not found" });
			}
			return Ok(property);
		}
		#endregion

		#region Insert Property
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpPost]
		public IActionResult InsertProperty([FromBody] PropertyModel propertyModel)
		{
			if (propertyModel == null)
			{
				return BadRequest(new { Message = "Invalid property data" });
			}
			if (propertyModel.Images == null || propertyModel.Images.Count == 0)
			{
				return BadRequest(new { Message = "At least one image is required." });
			}
			try
			{
				bool isInserted = _propertyRepository.Insert(propertyModel);
				if (isInserted)
				{
					return Ok(new { Message = "Property inserted successfully" });
				}
				return StatusCode(500, new { Message = "An error occurred while inserting the property" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = ex.Message });
			}
		}
		#endregion

		#region Update Property
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpPut("{id}")]
		public IActionResult UpdateProperty(int id, [FromBody] PropertyModel propertyModel)
		{
			if (propertyModel == null || id != propertyModel.PropertyID)
			{
				return BadRequest(new { Message = "Invalid property  ID" });
			}

			bool isUpdated = _propertyRepository.Update(propertyModel);
			if (isUpdated)
			{
				return Ok(new { Message = "Property updated successfully" });
			}
			return StatusCode(500, new { Message = "An error occurred while updating the property" });
		}
		#endregion

		#region Delete Property
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpDelete("{id}")]
		public IActionResult DeleteProperty(int id)
		{
			bool isDeleted = _propertyRepository.Delete(id);
			if (!isDeleted)
			{
				return NotFound(new { Message = "Property not found or already deleted" });
			}
			return Ok(new { Message = "Property deleted successfully" });
		}
		#endregion
	}
}
