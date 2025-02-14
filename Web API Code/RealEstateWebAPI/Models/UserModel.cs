namespace RealEstateWebAPI.Models
{
	public class UserModel
	{
		public int? UserID { get; set; }
		public string UserName { get; set; }
		public string? PhoneNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string? Password { get; set; }
		public string? Description { get; set; }
		public string UserRole { get; set; }
		public string? ProfilePhoto { get; set; }
		public string? Address { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public bool IsActive { get; set; }
	}
	public class Login
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string? Token { get; set; }
	}
	public class DecodeTokenRequest
	{
		public string Jwt { get; set; }
	}
	public class ProfilePhotoUpdate
	{
		public string ProfilePhoto { get; set; }
	}
	public class GoogleAuthRequest
	{
		public string Token { get; set; }
	}
	public class ChangePasswordModel
	{
		public int UserId { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
	}
	public class ForgotPasswordModel
	{
		public string Email { get; set; }
	}

	public class ResetPasswordModel
	{
		public string Email { get; set; }
		public string OTP { get; set; }
		public string NewPassword { get; set; }
	}

}
