using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Data;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	[Authorize(Roles = "Admin,Seller,Agent")]
	public class DashboardController : ControllerBase
	{
		private readonly DashboardRepository _dashboardRepository;

		public DashboardController(DashboardRepository dashboardRepository)
		{
			_dashboardRepository = dashboardRepository;
		}

		#region Get All Dashboard Data
		[HttpGet]
		public async Task<IActionResult> GetAllDashboard()
		{
			var dashboards = await _dashboardRepository.Index();
			return Ok(dashboards);
		}
		#endregion
		#region Get All Agent Dashboard Data
		[HttpGet("{userID}")]
		public async Task<IActionResult> GetAllDashboard(int userID)
		{
			var dashboards = await _dashboardRepository.GetRealEstateSummaryForAgentAsync(userID);
			if (dashboards == null)
			{
				return NotFound(new { message = "No data found for the specified agent." });
			}
			return Ok(dashboards);
		}
		#endregion
	}
}
