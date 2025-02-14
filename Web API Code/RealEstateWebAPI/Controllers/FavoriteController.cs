using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class FavoriteController : ControllerBase
	{
		private readonly FavoriteRepository _favoritesRepository;

		public FavoriteController(FavoriteRepository favoritesRepository)
		{
			_favoritesRepository = favoritesRepository;
		}

		#region Add Favorite
		[HttpPost]
		public IActionResult AddFavorite([FromBody] AddFavoriteModel model)
		{
			if (model == null || model.UserID <= 0 || model.PropertyID <= 0)
			{
				return BadRequest(new { Message = "Invalid data provided." });
			}

			bool isAdded = _favoritesRepository.AddFavorite(model.UserID, model.PropertyID);
			if (isAdded)
			{
				return Ok(new { Message = "Favorite added successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while adding the favorite." });
		}
		#endregion

		#region Remove Favorite
		[HttpDelete("{id}")]
		public IActionResult RemoveFavorite(int id)
		{
			bool isRemoved = _favoritesRepository.RemoveFavorite(id);
			if (!isRemoved)
			{
				return NotFound(new { Message = "Favorite not found or already removed." });
			}
			return Ok(new { Message = "Favorite removed successfully." });
		}
		#endregion

		#region Get Favorites by UserID
		[HttpGet("{userId}")]
		public IActionResult GetFavoritesByUser(int userId)
		{
			var favorites = _favoritesRepository.GetFavoritesByUser(userId);
			if (favorites == null || !favorites.Any())
			{
				return NotFound(new { Message = "No favorites found for the user." });
			}
			return Ok(favorites);
		}
		#endregion

	}
}
