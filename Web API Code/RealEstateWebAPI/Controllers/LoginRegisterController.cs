using Azure.Core;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Helper;
using RealEstateWebAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	[AllowAnonymous]
	public class LoginRegisterController : ControllerBase
	{
		private readonly JwtService _jwtService;
		private readonly LoginRegisterRepository _loginRegisterRepository;
		private readonly ILogger<LoginRegisterController> _logger;
		public LoginRegisterController(JwtService jwtService, LoginRegisterRepository userManager, ILogger<LoginRegisterController> logger)
		{
			_jwtService = jwtService;
			_loginRegisterRepository = userManager;
			_logger = logger;
		}

		// Registration endpoint
		[HttpPost("register")]
		public IActionResult Register([FromBody] UserModel userModel)
		{
			var existingUser = _loginRegisterRepository.LoginUser(userModel.UserName, userModel.Password);
			if (existingUser != null)
			{
				return BadRequest("Username or Email already exists");
			}

			var success = _loginRegisterRepository.RegisterUser(userModel);
			if (success)
			{
				return Ok("User registered successfully");
			}
			return BadRequest("Registration failed");
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] Login loginModel)
		{
			var user = _loginRegisterRepository.LoginUser(loginModel.UserName, loginModel.Password);
			if (user == null)
			{
				return Unauthorized("Invalid username or password.");
			}

			var token = _jwtService.GenerateJwtToken((int)user.UserID, user.UserName, user.UserRole,(user.FirstName + " " + user.LastName));

			user.Password = null;
			return Ok(new
			{
				Token = token,
				User = user
			});
		}

		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromBody] Login request)
		{
			try
			{
				var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
				_logger.LogInformation("Google token validated for {Email}", payload.Email);

				var existingUser = _loginRegisterRepository.GetUserByEmail(payload.Email);
				if(existingUser == null)
				{
					return BadRequest(new { message = "Not Register"});
				}
				var token = _jwtService.GenerateJwtToken((int)existingUser.UserID, existingUser.UserName, existingUser.UserRole, existingUser.FirstName + " " + existingUser.LastName);

				return Ok(new
				{
					Token = token,
					User = existingUser
				});
			}
			catch (Exception ex)
			{
				_logger.LogError("Google token validation failed: {Error}", ex.Message);
				return BadRequest(new { message = "Invalid Google token", error = ex.Message });
			}
		}


		// Endpoint to check if the user is authenticated (protected route)
		[HttpGet("profile")]
		public IActionResult GetProfile()
		{
			var token = HttpContext.Session.GetString("jwt");
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("No JWT token in session");
			}

			return Ok(new { Message = "Profile Data" });
		}

		#region Decode JWT
		[HttpPost("decode")]
		public IActionResult DecodeToken([FromBody] DecodeTokenRequest tokenRequest)
		{
			if (string.IsNullOrEmpty(tokenRequest.Jwt))
			{
				return BadRequest(new { error = "JWT token is required." });
			}
			try
			{
				var decodedToken = _jwtService.DecodeJwt(tokenRequest.Jwt);
				return Ok(decodedToken); // Returns the payload as JSON
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
		#endregion
	}
}
