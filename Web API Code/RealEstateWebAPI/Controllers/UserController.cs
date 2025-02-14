using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Models;
using System.Data;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly UserRepository _userRepository;
		private readonly EmailSender _emailService;

		public UserController(UserRepository userRepository,EmailSender emailSender)
		{
			_userRepository = userRepository;
			_emailService = emailSender;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetAllUsers()
		{
			var users = _userRepository.SelectAll();
			return Ok(users);
		}
		#region Get User By ID
		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetUserByID(int id)
		{
			var user = _userRepository.SelectByID(id);
			if (user == null)
			{
				return NotFound(new { Message = "User not found" });
			}
			return Ok(user);
		}
		#endregion

		#region Insert User
		[HttpPost]
		public IActionResult InsertUser([FromBody] UserModel userModel)
		{
			if (userModel == null)
			{
				return BadRequest(new { Message = "Invalid user data" });
			}

			bool isInserted = _userRepository.Insert(userModel);
			if (isInserted)
			{
				return Ok(new { Message = "User inserted successfully" });
			}
			return StatusCode(500, new { Message = "An error occurred while inserting the user" });
		}
		#endregion

		#region Update User
		[HttpPut("{id}")]
		public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
		{
			if (userModel == null || id != userModel.UserID)
			{
				return BadRequest(new { Message = "Invalid user ID" });
			}

			bool isUpdated = _userRepository.Update(userModel);
			if (isUpdated)
			{
				return Ok(new { Message = "User updated successfully" });
			}
			return StatusCode(500, new { Message = "An error occurred while updating the user" });
		}
		#endregion

		#region Delete User
		[HttpDelete("{id}")]
		public IActionResult DeleteUser(int id)
		{
			bool isDeleted = _userRepository.Delete(id);
			if (!isDeleted)
			{
				return NotFound(new { Message = "User not found or already deleted" });
			}
			return Ok(new { Message = "User deleted successfully" });
		}
		#endregion
		#region Deactivate User
		[HttpPut("{id}")]
		public IActionResult DeactivateUser(int id)
		{
			bool isDeactivated = _userRepository.DeactivateUser(id);
			if (!isDeactivated)
			{
				return NotFound(new { Message = "User not found or already deactivated" });
			}
			return Ok(new { Message = "User deactivated successfully" });
		}
		#endregion

		#region Update Profile Photo
		[HttpPut("{userID}")]
		public IActionResult UpdateProfilePhoto(int userID, [FromBody] ProfilePhotoUpdate request)
		{
			if (request == null || string.IsNullOrEmpty(request.ProfilePhoto))
			{
				return BadRequest("Invalid request data.");
			}

			bool isUpdated = _userRepository.UpdateProfilePhoto(userID, request.ProfilePhoto);

			if (isUpdated)
			{
				return Ok(new { message = "Profile photo updated successfully!" });
			}
			else
			{
				return NotFound();
			}
		}
		#endregion
		#region User Profile Photo
		[HttpGet("{id}")]
		public IActionResult GetUserProfilePhoto(int id)
		{
			var user = _userRepository.GetUserProfilePhoto(id);
			if (user == null)
			{
				return NotFound(new { Message = "User Profile Photo not found" });
			}
			return Ok(user);
		}
		#endregion
		#region Update/Change Password
		[HttpPost("ChangePassword")]
		public IActionResult ChangePassword([FromBody] ChangePasswordModel model)
		{
			if (model.NewPassword != model.ConfirmPassword)
			{
				return BadRequest("New password and confirmation password do not match.");
			}

			bool isSuccess = _userRepository.ChangePassword(model.UserId, model.OldPassword, model.NewPassword);

			if (!isSuccess)
			{
				return BadRequest("Incorrect old password or update failed.");
			}

			return Ok("Password changed successfully.");
		}
		#endregion

		#region Forgot Password
		[AllowAnonymous]
		[HttpPost("forgot-password")]
		public IActionResult ForgotPassword([FromBody] ForgotPasswordModel model)
		{
			Random random = new Random();
			string otp = random.Next(100000, 999999).ToString();
			DateTime expiry = DateTime.UtcNow.AddMinutes(5);

			bool result = _userRepository.SaveOTP(model.Email, otp, expiry);
			if (!result) return BadRequest("User not found");

			bool emailSent = _emailService.SendOTPEmail(model.Email, otp);
			if (!emailSent) return BadRequest("Failed to send OTP email.");

			return Ok(new { Message = "OTP sent to your email." });
		}
		#endregion
		#region Reset Password Based on Verify OTP
		[AllowAnonymous]
		[HttpPost("reset-password")]
		public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
		{
			//string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
			string result = _userRepository.VerifyOTPAndResetPassword(model.Email, model.OTP, model.NewPassword);

			if (result != "Success") return BadRequest(result);
			return Ok(new { Message = "Password reset successfully." });
		}
		#endregion
	}
}
