using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private readonly ReviewsRepository _reviewsRepository;

		public ReviewController(ReviewsRepository reviewsRepository)
		{
			_reviewsRepository = reviewsRepository;
		}

		#region Add Property Review
		[HttpPost]
		public IActionResult AddPropertyReview([FromBody] PropertyReview propertyReview)
		{
			if (propertyReview == null)
			{
				return BadRequest(new { Message = "Invalid review data provided." });
			}
			if (propertyReview.PropertyID == 0 || propertyReview.UserID == 0)
			{
				return BadRequest(new { Message = "Property and User must be provided." });
			}
			bool isAdded = _reviewsRepository.InsertPropertyReview(propertyReview);
			if (isAdded)
			{
				return Ok(new { Message = "Property review added successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while adding the property review." });
		}
		#endregion

		#region Add Agent Review
		[HttpPost]
		public IActionResult AddAgentReview([FromBody] AgentReview agentReview)
		{
			if (agentReview == null)
			{
				return BadRequest(new { Message = "Invalid review data provided." });
			}
			if (agentReview.AgentID == 0 || agentReview.UserID == 0)
			{
				return BadRequest(new { Message = "Agent and User must be provided." });
			}
			bool isAdded = _reviewsRepository.InsertAgentReview(agentReview);
			if (isAdded)
			{
				return Ok(new { Message = "Agent review added successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while adding the agent review." });
		}
		#endregion
		#region Get Agent Review by Agent ID
		[HttpGet("{agentId}")]
		public IActionResult GetAgentReviewByAgentID(int agentId)
		{
			var agentReviews = _reviewsRepository.GetAgentReviewByAgentID(agentId);
			if (agentReviews == null)
			{
				return NotFound(new { Message = "No reviews found for the agent." });
			}
			return Ok(agentReviews);
		}
		#endregion
		#region Get Property Review by Property ID
		[HttpGet("{propertyId}")]
		public IActionResult GetPropertyReviewByPropertyID(int propertyId)
		{
			var propertyReviews = _reviewsRepository.GetPropertyReviewByPropertyID(propertyId);
			if (propertyReviews == null)
			{
				return NotFound(new { Message = "No reviews found for the property." });
			}
			return Ok(propertyReviews);
		}
		#endregion
		#region Update Property Review
		[HttpPut]
		public IActionResult UpdatePropertyReview([FromBody] PropertyReview propertyReview)
		{
			if (propertyReview == null || propertyReview.ReviewID == 0)
			{
				return BadRequest(new { Message = "Invalid review data provided." });
			}

			bool isUpdated = _reviewsRepository.UpdatePropertyReview(propertyReview);
			if (isUpdated)
			{
				return Ok(new { Message = "Property review updated successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while updating the property review." });
		}
		#endregion

		#region Delete Property Review
		[HttpDelete("{reviewId}")]
		public IActionResult DeletePropertyReview(int reviewId)
		{
			if (reviewId == 0)
			{
				return BadRequest(new { Message = "Invalid review ID provided." });
			}

			bool isDeleted = _reviewsRepository.DeletePropertyReview(reviewId);
			if (isDeleted)
			{
				return Ok(new { Message = "Property review deleted successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while deleting the property review." });
		}
		#endregion
		#region Update Agent Review
		[HttpPut]
		public IActionResult UpdateAgentReview([FromBody] AgentReview agentReview)
		{
			if (agentReview == null || agentReview.ReviewID == 0)
			{
				return BadRequest(new { Message = "Invalid review data provided." });
			}

			bool isUpdated = _reviewsRepository.UpdateAgentReview(agentReview);
			if (isUpdated)
			{
				return Ok(new { Message = "Agent review updated successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while updating the agent review." });
		}
		#endregion

		#region Delete Agent Review
		[HttpDelete("{reviewId}")]
		public IActionResult DeleteAgentReview(int reviewId)
		{
			if (reviewId == 0)
			{
				return BadRequest(new { Message = "Invalid review ID provided." });
			}

			bool isDeleted = _reviewsRepository.DeleteAgentReview(reviewId);
			if (isDeleted)
			{
				return Ok(new { Message = "Agent review deleted successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while deleting the agent review." });
		}
		#endregion

	}
}
