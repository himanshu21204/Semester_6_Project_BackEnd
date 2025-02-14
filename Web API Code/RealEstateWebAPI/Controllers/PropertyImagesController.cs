using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	//[EnableCors("AllowMyOrigin")]
	public class PropertyImageController : ControllerBase
	{
		private readonly PropertyImagesRepository _propertyImageRepository;

		public PropertyImageController(PropertyImagesRepository propertyImageRepository)
		{
			_propertyImageRepository = propertyImageRepository;
		}

		#region Get All Images for a Property
		[HttpGet("{propertyId}")]
		public IActionResult GetImagesByPropertyID(int propertyId)
		{
			var images = _propertyImageRepository.GetByPropertyID(propertyId);
			if (images == null || !images.Any())
			{
				return NotFound(new { Message = "No images found for the specified property." });
			}
			return Ok(images);
		}
		#endregion

		#region Insert Property Image
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpPost]
		public IActionResult InsertImage([FromBody] PropertyImageModel imageModel)
		{
			if (imageModel == null || string.IsNullOrWhiteSpace(imageModel.ImageURL) || imageModel.PropertyID <= 0)
			{
				return BadRequest(new { Message = "Invalid image data." });
			}

			bool isInserted = _propertyImageRepository.Insert(imageModel.PropertyID, imageModel.ImageURL);
			if (isInserted)
			{
				return Ok(new { Message = "Image inserted successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while inserting the image." });
		}
		#endregion

		#region Delete Image by ImageID
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpDelete("image/{imageId}")]
		public IActionResult DeleteImageByImageID(int imageId)
		{
			bool isDeleted = _propertyImageRepository.DeleteByImageID(imageId);
			if (!isDeleted)
			{
				return NotFound(new { Message = "Image not found or already deleted." });
			}
			return Ok(new { Message = "Image deleted successfully." });
		}
		#endregion

		#region Delete All Images for a Property
		[Authorize(Roles = "Admin,Seller,Agent")]
		[HttpDelete("property/{propertyId}")]
		public IActionResult DeleteImagesByPropertyID(int propertyId)
		{
			bool isDeleted = _propertyImageRepository.DeleteByPropertyID(propertyId);
			if (!isDeleted)
			{
				return NotFound(new { Message = "No images found for the specified property or already deleted." });
			}
			return Ok(new { Message = "All images for the property deleted successfully." });
		}
		#endregion
	}
}
